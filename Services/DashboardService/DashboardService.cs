using FMSD_BE.Data;
using FMSD_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace FMSD_BE.Services.DashboardService
{
	public interface IDashboardService
	{
		Task<ResultWithMessage> GetCityReportAsync(string? name, bool tcv);
		Task<ResultWithMessage> GetStationReportAsync(string? name, bool tcv);
		//Task<ResultWithMessage> GetTankReportAsync(string? name, bool tcv);

		//Task<ResultWithMessage> GetStationsReportAsync();
	}

	public class DashboardService(CentralizedFmsCloneContext db) : IDashboardService
	{
		private readonly CentralizedFmsCloneContext _db = db;

		public async Task<ResultWithMessage> GetStationReportAsync(string? name, bool tcv)
		{
			var stations = _db.Stations.Where(station => station.DeletedAt == null);

			if (!string.IsNullOrEmpty(name))
				stations = stations.Where(e => e.City.Trim().ToLower() == name.Trim().ToLower());


			var query = stations.Select(station => new
			{
				Station = station,
				Tanks = station.Tanks
											.Where(tank => tank.TankStatusId == 2)
											.Select(tank => new
											{
												Tank = tank,
												LastMeasurement = tank.TankMeasurements
													.OrderByDescending(tm => tm.Id)
													.FirstOrDefault(),
											})
			}).OrderBy(e => e.Station.StationName);



			var stationDetails = await query
				.GroupBy(g => g.Station.StationName)
				.Select(e => new
				{
					stationName = e.Key,
					fuelVolume = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum(),
					remeningFuel = (_db.Tanks.Where(t => t.Station.StationName == e.Key).Select(c => c.Capacity).Sum()) - (e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum()),

					BackgroundColor = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum() /
									(_db.Tanks.Where(t => t.Station.StationName == e.Key).Select(c => c.Capacity).Sum()) <= 0.2 ? "rgba(255, 99, 132, 0.2)" :
									e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum() /
									(_db.Tanks.Where(t => t.Station.StationName == e.Key).Select(c => c.Capacity).Sum()) <= 0.5 ? "rgba(255, 159, 64, 0.2)" : "rgba(75, 192, 192, 0.2)",

					TCV = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum(),
					TCVRemaining = (_db.Tanks.Where(t => t.Station.StationName == e.Key).Select(c => c.Capacity).Sum()) - (e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum()),

					TCVBacgroundColor = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum() /
									(_db.Tanks.Where(t => t.Station.StationName == e.Key).Select(c => c.Capacity).Sum()) <= 0.2 ? "rgba(255, 99, 132, 0.2)" :
									e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum() /
									(_db.Tanks.Where(t => t.Station.StationName == e.Key).Select(c => c.Capacity).Sum()) <= 0.5 ? "rgba(255, 159, 64, 0.2)" : "rgba(75, 192, 192, 0.2)"
				})
				.OrderBy(e => e.stationName)
				.ToListAsync();

			var result = new ChartApiResponse
			{
				Datasets =
					new List<DataSetModel>
					{
						new DataSetModel
						{
							Data = tcv ? stationDetails.Select(e=>e.TCV).ToList() : stationDetails.Select(e=>e.fuelVolume).ToList(),
							Label = "Fuel Level",
							BackgroundColor=tcv ?stationDetails.Select(e=>e.TCVBacgroundColor).ToList():  stationDetails.Select(e=>e.BackgroundColor).ToList(),
							BorderColor = tcv ?stationDetails.Select(e=>e.TCVBacgroundColor).ToList():  stationDetails.Select(e=>e.BackgroundColor).ToList(),
							Fill = "start",
							Stack="a"
						},
						new DataSetModel
						{
							Data = stationDetails.Select(e=>e.remeningFuel).ToList(),
							Label = "Remaining",
							BackgroundColor= new List<string>{"rgba(108, 122, 137)"},
							//BorderColor = stationDetails.Select(e=>e.BackgroundColor).ToList(),
							Fill = "start",
							Stack="a"
						}
				},

				Labels =

					stationDetails.Select(e => e.stationName).ToList()
				//stationDetails.Select(e=>e.stationName).ToList()
				,

				Values = []
			};


			return new ResultWithMessage(result, string.Empty);
		}

		public async Task<ResultWithMessage> GetCityReportAsync(string? name, bool tcv)
		{
			var stations = _db.Stations.Where(station => station.DeletedAt == null);

			if (!string.IsNullOrEmpty(name))
				stations = stations.Where(e => e.City.Trim().ToLower() == name.Trim().ToLower());


			var query = stations.Select(station => new
			{
				Station = station,
				Tanks = station.Tanks
											.Where(tank => tank.TankStatusId == 2)
											.Select(tank => new
											{
												Tank = tank,
												LastMeasurement = tank.TankMeasurements
													.OrderByDescending(tm => tm.Id)
													.FirstOrDefault(),
											})
			}).OrderBy(e => e.Station.StationName);



			var stationDetails = await query
				.GroupBy(g => g.Station.City)
				.Select(e => new
				{
					stationName = e.Key,
					fuelVolume = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum(),
					remeningFuel = (_db.Tanks.Where(t => t.Station.StationName == e.Key).Select(c => c.Capacity).Sum()) - (e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum()),

					BackgroundColor = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum() /
									(_db.Tanks.Where(t => t.Station.StationName == e.Key).Select(c => c.Capacity).Sum()) <= 0.2 ? "rgba(255, 99, 132, 0.2)" :
									e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum() /
									(_db.Tanks.Where(t => t.Station.StationName == e.Key).Select(c => c.Capacity).Sum()) <= 0.5 ? "rgba(255, 159, 64, 0.2)" : "rgba(75, 192, 192, 0.2)",

					TCV = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum(),
					TCVRemaining = (_db.Tanks.Where(t => t.Station.StationName == e.Key).Select(c => c.Capacity).Sum()) - (e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum()),

					TCVBacgroundColor = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum() /
									(_db.Tanks.Where(t => t.Station.StationName == e.Key).Select(c => c.Capacity).Sum()) <= 0.2 ? "rgba(255, 99, 132, 0.2)" :
									e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum() /
									(_db.Tanks.Where(t => t.Station.StationName == e.Key).Select(c => c.Capacity).Sum()) <= 0.5 ? "rgba(255, 159, 64, 0.2)" : "rgba(75, 192, 192, 0.2)"
				})
				.OrderBy(e => e.stationName)
				.ToListAsync();

			var result = new ChartApiResponse
			{
				Datasets =
					new List<DataSetModel>
					{
						new DataSetModel
						{
							Data = tcv ? stationDetails.Select(e=>e.TCV).ToList() : stationDetails.Select(e=>e.fuelVolume).ToList(),
							Label = "Fuel Level",
							BackgroundColor=tcv ?stationDetails.Select(e=>e.TCVBacgroundColor).ToList():  stationDetails.Select(e=>e.BackgroundColor).ToList(),
							BorderColor = tcv ?stationDetails.Select(e=>e.TCVBacgroundColor).ToList():  stationDetails.Select(e=>e.BackgroundColor).ToList(),
							Fill = "start",
							Stack="a"
						},
						new DataSetModel
						{
							Data = stationDetails.Select(e=>e.remeningFuel).ToList(),
							Label = "Remaining",
							BackgroundColor= new List<string>{"rgba(108, 122, 137)"},
							//BorderColor = stationDetails.Select(e=>e.BackgroundColor).ToList(),
							Fill = "start",
							Stack="a"
						}
				},

				Labels =

					stationDetails.Select(e => e.stationName).ToList()
				//stationDetails.Select(e=>e.stationName).ToList()
				,

				Values = []
			};


			return new ResultWithMessage(result, string.Empty);
		}
	}
}
