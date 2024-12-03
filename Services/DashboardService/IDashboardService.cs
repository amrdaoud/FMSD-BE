using FMSD_BE.Helper;

namespace FMSD_BE.Services.DashboardService
{
	public interface IDashboardService
	{
		Task<ResultWithMessage> CityReportAsync(string? name, bool tcv);
		Task<ResultWithMessage> StationReportAsync(string? name, bool tcv);
		Task<ResultWithMessage> TankReportAsync(string? name, bool tcv);
		Task<ResultWithMessage> TanksDailyFuelVolumeAsync(DateTime startDate, DateTime endDate);
		Task<ResultWithMessage> AlarmTypesChartAsync(DateTime startDate, DateTime endDate);
		Task<ResultWithMessage> DailyLeackageChartAsync(string? name, DateTime startDate, DateTime endDate);
		Task<ResultWithMessage> DailyLeackagesPerStationChartAsync(string? name, DateTime startDate, DateTime endDate);
		Task<ResultWithMessage> DailyLeackagesPerTankChartAsync(string? name, DateTime startDate, DateTime endDate);
	}
}
