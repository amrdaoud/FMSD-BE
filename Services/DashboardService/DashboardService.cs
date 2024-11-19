using FMSD_BE.Data;
using FMSD_BE.Dtos.DashboardDtos;
using FMSD_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace FMSD_BE.Services.DashboardService
{
	public class DashboardService(CentralizedFmsCloneContext db) : IDashboardService
	{
		private readonly CentralizedFmsCloneContext _db = db;

		public async Task<ResultWithMessage> CityReportAsync(string? name, bool tcv)
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
						//Name = "Fill%",
						//Value = Math.Round (stationDetails.Select(e=>e.fuelVolume).Sum() / stationDetails.Select(e=>e.capacity).Sum()* 100).ToString() + "%"
						Name = "Volume/L",
						Value = Math.Round (stationDetails.Select(e=>e.fuelVolume).Sum() / 1000).ToString() + "K"
					},
					new LookUpResponse {
						//Name = "TCV Fill%",
						//Value = Math.Round (stationDetails.Select(e=>e.tcv).Sum() / stationDetails.Select(e=>e.capacity).Sum() * 100).ToString() + "%"
						Name = "TCV Volume/L",
						Value = Math.Round (stationDetails.Select(e=>e.tcv).Sum() / 1000).ToString() + "K"
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
		public async Task<ResultWithMessage> StationReportAsync(string? name, bool tcv)
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
						//Name = "Fill%",
						//Value = Math.Round (stationDetails.Select(e=>e.fuelVolume).Sum() / stationDetails.Select(e=>e.capacity).Sum()* 100).ToString() + "%"
						Name = "Volume/L",
						Value = Math.Round (stationDetails.Select(e=>e.fuelVolume).Sum() /1000).ToString() + "K"
					},
					new LookUpResponse {
						//Name = "TCV Fill%",
						//Value = Math.Round (stationDetails.Select(e=>e.tcv).Sum() / stationDetails.Select(e=>e.capacity).Sum() * 100).ToString() + "%"
						Name = "TCV Volume/L",
						Value = Math.Round (stationDetails.Select(e=>e.tcv).Sum() / 1000).ToString() + "K"
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
		public async Task<ResultWithMessage> TankReportAsync(string? name, bool tcv)
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
						//Name = "Fill%",
						//Value = Math.Round (tankDetails.Select(e=>e.LastMeasurement.fuelVolume).Sum() / tankDetails.Select(e=>e.LastMeasurement.capacity).Sum() * 100).ToString() + "%"
						Name = "Volume/L",
						Value = Math.Round (tankDetails.Select(e=>e.LastMeasurement.fuelVolume).Sum() / 1000).ToString() + "K"
					},
					new LookUpResponse {
						//Name = "TCV Fill%",
						//Value = Math.Round (tankDetails.Select(e=>e.LastMeasurement.tcv).Sum()/ tankDetails.Select(e=>e.LastMeasurement.capacity).Sum() * 100).ToString() + "%"
						Name = "TCV Volume/L",
						Value = Math.Round (tankDetails.Select(e=>e.LastMeasurement.tcv).Sum()/ 1000).ToString() + "K"
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
		public async Task<ResultWithMessage> TanksDailyFuelVolumeAsync(DateTime startDate, DateTime endDate)
		{
			if (startDate != null && endDate != null)
			{
				startDate = Utilites.convertDateToArabStandardDate((DateTime)startDate);
				endDate = Utilites.convertDateToArabStandardDate((DateTime)endDate).AddDays(1).AddSeconds(-1);
			}


			////////////////////////////////////////////// Soultion1  //////////////////////////////////////////////

			//var maxMeasures = _db.TankMeasurements
			//	.Where(e => e.CreatedAt.HasValue && e.CreatedAt >= request.StartDate && e.CreatedAt <= request.EndDate)
			//	.GroupBy(g => new
			//	{
			//		Day = g.CreatedAt.Value.Date,
			//		g.TankGuid
			//	})
			//	.Select(e => e.Key.Day == request.StartDate.Date ? e.Min(i => i.Id) : e.Max(i => i.Id));

			//var tanksPerDay = await _db.TankMeasurements
			//	.Where(e => maxMeasures.Contains(e.Id))
			//	.GroupBy(g => g.CreatedAt.Value.Date)
			//	.Select(e => new
			//	{
			//		Date = e.Key,
			//		fuelLevel = e.Sum(f => f.FuelLevel),
			//		fuelVolume = e.Sum(f => f.FuelVolume),
			//		capacity = e.Sum(t => t.Tank.Capacity),
			//		//backgroundColor = e.Sum(f => f.FuelVolume) / e.Sum(t => t.Tank.Capacity) <= 0.2 ? "rgba(255, 99, 132, 0.2)" : e.Sum(f => f.FuelVolume) / e.Sum(t => t.Tank.Capacity) <= 0.5 ? "rgba(255, 159, 64, 0.2)" : "rgba(75, 192, 192, 0.2)"
			//	})
			//	.OrderBy(e => e.Date)
			//	.ToListAsync();


			//var result = new ChartApiResponse
			//{
			//	Datasets =
			//		[
			//			new DataSetModel
			//			{
			//				Data = tanksPerDay.Select(e=>e.fuelVolume).ToList(),
			//				Label = "Fuel Volume",
			//				BackgroundColor= ["#00cccc33"],
			//				BorderColor =["#00cccc"],
			//				Fill = "start",
			//			}
			//	],
			//	Labels = tanksPerDay.Select(e => e.Date.ToString()).ToList(),

			//	Values = [],
			//};

			//result.CardValue = new CardValue
			//{
			//	IsUp = result.Datasets[0].Data.LastOrDefault() - result.Datasets[0].Data.FirstOrDefault() > 0 ? true : false,
			//	BoldValueTitle = "Available Rate",
			//	//BoldValue = "FuelAll / AllCapacity * 100 for request last date",
			//	BoldValue = Math.Round(((tanksPerDay.Select(e => e.fuelVolume).LastOrDefault() / tanksPerDay.Select(e => e.capacity).LastOrDefault()) * 100)).ToString() + "%",

			//	//LightValue = "Math.ABS" + "FuelAll / AllCapacity * 100 for request last date - FuelAll / AllCapacity * 100 for request First date"
			//	LightValue = Math.Abs(Math.Round(((tanksPerDay.Select(e => e.fuelVolume).LastOrDefault() / tanksPerDay.Select(e => e.capacity).LastOrDefault()) * 100) - ((tanksPerDay.Select(e => e.fuelVolume).FirstOrDefault() / tanksPerDay.Select(e => e.capacity).FirstOrDefault()) * 100))).ToString() + "%"
			//};

			////////////////////////////////////////////// Soultion2  //////////////////////////////////////////////


			//1) get all tanks
			var tankGuids = await _db.Tanks
				.Where(e => e.TankStatusId == 2 && e.Station.DeletedAt == null && !string.IsNullOrEmpty(e.Station.StationType))
				.Select(e => e.Guid)
				.Distinct()
				.OrderBy(tankGuid => tankGuid)
				.ToListAsync();

			//2) get date ranges
			List<DateOnly> dateRanges = [];
			for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
				dateRanges.Add(DateOnly.FromDateTime(date));

			//3) tanks Per Days
			List<DailyFuelVolumeViewModel> tanksPerDays = [];

			foreach (var date in dateRanges)
			{
				var tanksPerDay = new DailyFuelVolumeViewModel
				{
					Date = date,
					FuelVolume = 0,
					Capacity = 0
				};

				foreach (var tankGuid in tankGuids)
				{

					var res = _db.TankMeasurements
						.Include(t => t.Tank)
						.Where(e => e.TankGuid == tankGuid && DateOnly.FromDateTime((DateTime)e.CreatedAt) == date)
						.OrderByDescending(e => e.CreatedAt)
						.FirstOrDefault();

					if (res is not null)
					{
						tanksPerDay.FuelVolume += res.FuelVolume;
						tanksPerDay.Capacity += res.Tank.Capacity;
					}
					else
					{

						var lastMeasurement = _db.TankMeasurements
						.Include(t => t.Tank)
						.Where(e => e.TankGuid == tankGuid)
						.OrderByDescending(e => e.CreatedAt)
						.FirstOrDefault();

						tanksPerDay.FuelVolume += lastMeasurement.FuelVolume;
						tanksPerDay.Capacity += lastMeasurement.Tank.Capacity;
					}
				}

				tanksPerDays.Add(tanksPerDay);
			}


			var result = new ChartApiResponse
			{
				Datasets =
					[
						new DataSetModel
						{
							Data = tanksPerDays.Select(e=>e.FuelVolume).ToList(),
							Label = "Fuel Volume",
							BackgroundColor= ["#00cccc33"],
							BorderColor =["#00cccc"],
							Fill = "start",
						}
				],
				Labels = tanksPerDays.Select(e => e.Date.ToString()).ToList(),

				Values = [],
			};

			var isUp = result.Datasets[0].Data.LastOrDefault() - result.Datasets[0].Data.FirstOrDefault() > 0;

			result.CardValue = new CardValue
			{
				Icon = new Icon
				{
					Text = isUp ? "trending_up" : "trending_down",
					Color = isUp ? "#3BB001" : "#DC3545"
				},

				BoldValueTitle = "Available Rate",
				//BoldValue = "FuelAll / AllCapacity * 100 for request last date",
				BoldValue = Math.Round(((tanksPerDays.Select(e => e.FuelVolume).LastOrDefault() / tanksPerDays.Select(e => e.Capacity).LastOrDefault()) * 100)).ToString() + "%",

				//LightValue = "Math.ABS" + "FuelAll / AllCapacity * 100 for request last date - FuelAll / AllCapacity * 100 for request First date"
				LightValue = Math.Abs(Math.Round(((tanksPerDays.Select(e => e.FuelVolume).LastOrDefault() / tanksPerDays.Select(e => e.Capacity).LastOrDefault()) * 100) - ((tanksPerDays.Select(e => e.FuelVolume).FirstOrDefault() / tanksPerDays.Select(e => e.Capacity).FirstOrDefault()) * 100))).ToString() + "%"
			};

			return new ResultWithMessage(result, string.Empty);
		}
	}
}