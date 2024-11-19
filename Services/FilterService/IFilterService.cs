using FMSD_BE.Dtos.DashboardDtos;
using FMSD_BE.Helper;

namespace FMSD_BE.Services.FilterService
{
	public interface IFilterService
	{
		Task<ResultWithMessage> GetAllCitiesAsync();
		Task<ResultWithMessage> GetAllStationsAsync(string? name);
		Task<ResultWithMessage> GetAllAlarmTypesAsync();
		Task<ResultWithMessage> GetTransactionStatusesAsync();
		Task<ResultWithMessage> GetAllTanksAsync(GetTanksRequest request);
	}
}
