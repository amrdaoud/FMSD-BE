using FMSD_BE.Data;
using FMSD_BE.Dtos.DashboardDtos;
using FMSD_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace FMSD_BE.Services.FilterService
{
	public class FilterService(CentralizedFmsCloneContext db) : IFilterService
	{
		private readonly CentralizedFmsCloneContext _db = db;

		public async Task<ResultWithMessage> GetAllCitiesAsync()
		{
			var result = await _db.Stations
				.Select(e => e.City)
				.Distinct()
				.OrderBy(city => city)
				.ToListAsync();

			return new ResultWithMessage(result, string.Empty);
		}
		public async Task<ResultWithMessage> GetAllStationsAsync(string? name)
		{
			var query = _db.Stations.Where(e => e.DeletedAt == null && !string.IsNullOrEmpty(e.StationType));

			if (!string.IsNullOrEmpty(name))
				query = query.Where(e => e.City.Trim().ToLower().Contains(name.Trim().ToLower()));

			var stations = await query
				.Select(e => new StationListViewModel
				{
					Guid = e.Guid,
					Id = e.Id,
					City = e.City,
					CreatedAt = e.CreatedAt,
					DeletedAt = e.DeletedAt,
					PumpNumber = e.PumpNumber,
					StationName = e.StationName,
					StationType = e.StationType,
					TankNumber = e.TankNumber,
					UpdatedAt = e.UpdatedAt
				})
				.OrderBy(e => e.StationName)
				.ToListAsync();

			return new ResultWithMessage(stations, string.Empty);


		}
		public async Task<ResultWithMessage> GetAllAlarmTypesAsync()
		{
			var result = await _db.Alarms
				.Select(e => e.Type)
				.Distinct()
				.OrderBy(alarmType => alarmType)
				.ToListAsync();

			return new ResultWithMessage(result, string.Empty);
		}
		public async Task<ResultWithMessage> GetTransactionStatusesAsync()
		{
			var result = await _db.TransactionStatuses
				.Select(e => e.TransStatus)
				.Distinct()
				.OrderBy(status => status)
				.ToListAsync();

			return new ResultWithMessage(result, string.Empty);
		}
		public async Task<ResultWithMessage> GetAllTanksAsync(GetTanksRequest request)
		{
			var query = _db.Tanks
				.Where(e => e.TankStatusId == 2 &&
							e.Station.DeletedAt == null &&
							!string.IsNullOrEmpty(e.Station.StationType));

			if (!string.IsNullOrEmpty(request.CityName))
				query = query.Where(e => e.Station.City.Contains(request.CityName.Trim().ToLower()));

			if (!string.IsNullOrEmpty(request.StationGuid))
				query = query.Where(e => e.Station.Guid.ToString().Contains(request.StationGuid.Trim().ToLower()));

			var result = await query
				.Select(e => new TankListViewModel
				{
					StationName = e.Station.StationName,
					City = e.Station.City,
					TankName = e.TankName,
					Capacity = e.Capacity,
					CreatedAt = e.CreatedAt,
					Description = e.Description,
					DLLimit = e.DLLimit,
					Guid = e.Guid.ToString(),
					Height = e.Height,
					HighLimit = e.HighLimit,
					HighHighLimit = e.HighHighLimit,
					Hysteresis = e.Hysteresis,
					Id = e.Id,
					LogicalAddress = e.LogicalAddress,
					LowLimit = e.LowLimit,
					LowLowLimit = e.LowLowLimit,
					MLLimit = e.MLLimit,
					PhysicalAddress = e.PhysicalAddress,
					StationGuid = e.StationGuid.ToString(),
					TankStatus = e.TankStatus!.Name,
					TankStatusId = e.TankStatusId,
					UpdatedAt = e.UpdatedAt,
					WaterHighLimit = e.WaterHighLimit,
					WLLimit = e.MLLimit
				})
				.OrderBy(e => e.StationName)
				.ThenBy(e => e.TankName)
				.ToListAsync();

			return new ResultWithMessage(result, string.Empty);
		}
	}
}
