using FMSD_BE.Dtos.DashboardDtos;
using FMSD_BE.Helper;

namespace FMSD_BE.Services.DashboardService
{
	public interface IDashboardService
	{
		Task<ResultWithMessage> GetCityReportAsync(string? name, bool tcv);
		Task<ResultWithMessage> GetStationReportAsync(string? name, bool tcv);
		Task<ResultWithMessage> GetTankReportAsync(string? name, bool tcv);
		Task<ResultWithMessage> TanksDailyFuelLevelAsync(DailyFuelLevelRequest request);
		Task<ResultWithMessage> GetAllCitiesAsync();
		Task<ResultWithMessage> GetAllStationsAsync(string? name);
		Task<ResultWithMessage> GetAllAlarmTypesAsync();
	}
}
