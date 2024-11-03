using FMSD_BE.Data;
using FMSD_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace FMSD_BE.Services.DashboardService
{
	public interface IDashboardService
	{
		Task<ResultWithMessage> GetCityReportAsync(string? name, bool tcv);
		Task<ResultWithMessage> GetStationReportAsync(string? name, bool tcv);
		Task<ResultWithMessage> GetTankReport(string? name, bool tcv);
	}

	public class DashboardService(CentralizedFmsCloneContext db) : IDashboardService
	{
		private readonly CentralizedFmsCloneContext _db = db;

		public async Task<ResultWithMessage> GetCityReportAsync(string? name, bool tcv)
		{
			var stations = _db.Stations.Where(station => station.DeletedAt == null && !string.IsNullOrEmpty(station.StationType));

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
			})
			.OrderBy(e => e.Station.StationName);

			var stationDetails = await query
				.GroupBy(g => g.Station.City)
				.Select(e => new
				{
					city = e.Key,
					fuelVolume = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum(),
					capacity = e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum(),
					remeningFuel = e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum() - e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum(),
					tcv = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum(),
					tcvRemaining = e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum() - e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum(),
					temperature = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Temperature)).Max(),
					waterVolume = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.WaterVolume)).Sum(),

					BackgroundColor = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum() / e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum() <= 0.2 ? "rgba(255, 99, 132, 0.2)" :
										e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum() / e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum() <= 0.5 ? "rgba(255, 159, 64, 0.2)" :
										"rgba(75, 192, 192, 0.2)",

					TCVBacgroundColor = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum() / e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum() <= 0.2 ? "rgba(255, 99, 132, 0.2)" :
										e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum() / e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum() <= 0.5 ? "rgba(255, 159, 64, 0.2)" :
										"rgba(75, 192, 192, 0.2)"
				})
				.OrderBy(e => e.city)
				.ToListAsync();


			var result = new ChartApiResponse
			{
				Datasets =
					[
						new DataSetModel
						{
							Data = tcv ? stationDetails.Select(e=>e.tcv).ToList() : stationDetails.Select(e=>e.fuelVolume).ToList(),
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
							BackgroundColor= ["rgba(108, 122, 137)"],
							BorderColor = stationDetails.Select(e=>e.BackgroundColor).ToList(),
							Fill = "start",
							Stack="a"
						}
				],
				Labels = stationDetails.Select(e => e.city).ToList(),

				Values =
				[
					new LookUpResponse {
						Name = "Fill%",
						Value = Math.Round (stationDetails.Select(e=>e.fuelVolume).Sum() / stationDetails.Select(e=>e.capacity).Sum()* 100).ToString() + "%"
					},
					new LookUpResponse {
						Name = "TCV Fill%",
						Value = Math.Round (stationDetails.Select(e=>e.tcv).Sum() / stationDetails.Select(e=>e.capacity).Sum() * 100).ToString() + "%"
					},
					new LookUpResponse {
						Name = "Max Temperature",
						Value = stationDetails.Select(e=>e.temperature).Max().ToString()
					},
					new LookUpResponse {
						Name = "Max Water%",
						Value = Math.Round (stationDetails.Select(e=>e.waterVolume).Sum() / stationDetails.Select(e=>e.fuelVolume).Sum() * 100).ToString() + "%"
					},
				]
			};

			return new ResultWithMessage(result, string.Empty);
		}
		public async Task<ResultWithMessage> GetStationReportAsync(string? name, bool tcv)
		{
			var stations = _db.Stations.Where(station => station.DeletedAt == null && !string.IsNullOrEmpty(station.StationType));

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
			})
			.OrderBy(e => e.Station.StationName);

			var stationDetails = await query
				.GroupBy(g => g.Station.StationName)
				.Select(e => new
				{
					stationName = e.Key,
					fuelVolume = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum(),
					capacity = e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum(),
					remeningFuel = e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum() - e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum(),
					tcv = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum(),
					tcvRemaining = e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum() - e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum(),
					temperature = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Temperature)).Max(),
					waterVolume = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.WaterVolume)).Sum(),

					BackgroundColor = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum() / e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum() <= 0.2 ? "rgba(255, 99, 132, 0.2)" :
										e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.FuelVolume)).Sum() / e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum() <= 0.5 ? "rgba(255, 159, 64, 0.2)" :
										"rgba(75, 192, 192, 0.2)",

					TCVBacgroundColor = e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum() / e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum() <= 0.2 ? "rgba(255, 99, 132, 0.2)" :
										e.SelectMany(t => t.Tanks.Select(l => l.LastMeasurement.Tcv)).Sum() / e.SelectMany(t => t.Tanks.Select(c => c.Tank.Capacity)).Sum() <= 0.5 ? "rgba(255, 159, 64, 0.2)" :
										"rgba(75, 192, 192, 0.2)"
				})
				.OrderBy(e => e.stationName)
				.ToListAsync();


			var result = new ChartApiResponse
			{
				Datasets =
					[
						new DataSetModel
						{
							Data = tcv ? stationDetails.Select(e=>e.tcv).ToList() : stationDetails.Select(e=>e.fuelVolume).ToList(),
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
							BackgroundColor= ["rgba(108, 122, 137)"],
							BorderColor = stationDetails.Select(e=>e.BackgroundColor).ToList(),
							Fill = "start",
							Stack="a"
						}
				],
				Labels = stationDetails.Select(e => e.stationName).ToList(),

				Values =
				[
					new LookUpResponse {
						Name = "Fill%",
						Value = Math.Round (stationDetails.Select(e=>e.fuelVolume).Sum() / stationDetails.Select(e=>e.capacity).Sum()* 100).ToString() + "%"
					},
					new LookUpResponse {
						Name = "TCV Fill%",
						Value = Math.Round (stationDetails.Select(e=>e.tcv).Sum() / stationDetails.Select(e=>e.capacity).Sum() * 100).ToString() + "%"
					},
					new LookUpResponse {
						Name = "Max Temperature",
						Value = stationDetails.Select(e=>e.temperature).Max().ToString()
					},
					new LookUpResponse {
						Name = "Max Water%",
						Value = Math.Round (stationDetails.Select(e=>e.waterVolume).Sum() / stationDetails.Select(e=>e.fuelVolume).Sum() * 100).ToString() + "%"
					},
				]
			};

			return new ResultWithMessage(result, string.Empty);
		}
		public async Task<ResultWithMessage> GetTankReport(string? name, bool tcv)
		{
			var query = _db.Tanks.Where(e => e.TankStatusId == 2 && e.Station.DeletedAt == null && !string.IsNullOrEmpty(e.Station.StationType));

			if (!string.IsNullOrEmpty(name))
				query = query.Where(e => e.Station.StationName.Trim().ToLower() == name.Trim().ToLower());

			var tankDetails = await query
				.Select(t => new
				{
					tankName = t.Station.StationName + "/" + t.TankName,
					LastMeasurement = t.TankMeasurements
										.OrderByDescending(tm => tm.Id)
										.Select(lm => new
										{
											capacity = t.Capacity,
											fuelVolume = lm.FuelVolume,
											remainingFuel = t.Capacity - lm.FuelVolume,
											tcv = lm.Tcv,
											tcvRemaining = t.Capacity - lm.Tcv,
											temperature = lm.Temperature,
											waterVolume = lm.WaterVolume,

											backgroundColor = lm.FuelVolume / t.Capacity <= 0.2 ? "rgba(255, 99, 132, 0.2)" :
															 lm.FuelVolume / t.Capacity <= 0.5 ? "rgba(255, 159, 64, 0.2)" :
															 "rgba(75, 192, 192, 0.2)",

											tcvBacgroundColor = lm.Tcv / t.Capacity <= 0.2 ? "rgba(255, 99, 132, 0.2)" :
																lm.Tcv / t.Capacity <= 0.5 ? "rgba(255, 159, 64, 0.2)" :
																"rgba(75, 192, 192, 0.2)"
										})
										.FirstOrDefault()
				})
				.OrderBy(e => e.tankName)
				.ToListAsync();

			var result = new ChartApiResponse
			{
				Datasets =
					[
						new DataSetModel
						{
							Data = tcv ? tankDetails.Select(e=>e.LastMeasurement.tcv).ToList() : tankDetails.Select(e=>e.LastMeasurement.fuelVolume).ToList(),
							Label = "Fuel Level",
							BackgroundColor=tcv ? tankDetails.Select(e=>e.LastMeasurement.tcvBacgroundColor).ToList() :   tankDetails.Select(e=>e.LastMeasurement.backgroundColor).ToList(),
							BorderColor = tcv ? tankDetails.Select(e=>e.LastMeasurement.tcvBacgroundColor).ToList() :   tankDetails.Select(e=>e.LastMeasurement.backgroundColor).ToList(),
							Fill = "start",
							Stack="a"
						},
						new DataSetModel
						{
							Data = tankDetails.Select(e=>e.LastMeasurement.remainingFuel).ToList(),
							Label = "Remaining",
							BackgroundColor= ["rgba(108, 122, 137)"],
							BorderColor = tankDetails.Select(e=>e.LastMeasurement.tcvBacgroundColor).ToList() ,
							Fill = "start",
							Stack="a"
						}
				],
				Labels = tankDetails.Select(e => e.tankName).ToList(),

				Values =
				[
					new LookUpResponse {
						Name = "Fill%",
						Value = Math.Round (tankDetails.Select(e=>e.LastMeasurement.fuelVolume).Sum() / tankDetails.Select(e=>e.LastMeasurement.capacity).Sum() * 100).ToString() + "%"
					},
					new LookUpResponse {
						Name = "TCV Fill%",
						Value = Math.Round (tankDetails.Select(e=>e.LastMeasurement.tcv).Sum()/ tankDetails.Select(e=>e.LastMeasurement.capacity).Sum() * 100).ToString() + "%"
					},
					new LookUpResponse {
						Name = "Max Temperature",
						Value = tankDetails.Select(e=>e.LastMeasurement.temperature).Max().ToString()
					},
					new LookUpResponse {
						Name = "Max Water%",
						Value = Math.Round (tankDetails.Select(e=>e.LastMeasurement.waterVolume).Sum() / tankDetails.Select(e=>e.LastMeasurement.fuelVolume).Sum() * 100).ToString() + "%"
					}
				]
			};

			return new ResultWithMessage(result, string.Empty);
		}
	}
}