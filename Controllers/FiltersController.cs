using FMSD_BE.Services.FilterService;
using Microsoft.AspNetCore.Mvc;

namespace FMSD_BE.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FiltersController(IFilterService filterService) : ControllerBase
	{
		private readonly IFilterService _filterService = filterService;

		[HttpGet("Cities")]
		public async Task<IActionResult> Cities()
		{
			var result = await _filterService.GetAllCitiesAsync();

			return Ok(result.Data);
		}


		[HttpGet("Stations")]
		public async Task<IActionResult> Stations(string? name)
		{
			var result = await _filterService.GetAllStationsAsync(name);

			return Ok(result.Data);
		}


		[HttpGet("AlarmTypes")]
		public async Task<IActionResult> AlarmTypes()
		{
			var result = await _filterService.GetAllAlarmTypesAsync();

			return Ok(result.Data);
		}


		[HttpGet("TransactionStatuses")]
		public async Task<IActionResult> TransactionStatuses()
		{
			var result = await _filterService.GetAllTransactionStatusesAsync();

			return Ok(result.Data);
		}


		[HttpGet("Tanks")]
		public async Task<IActionResult> Tanks(string? cityName, string? stationGuid)
		{
			var result = await _filterService.GetAllTanksAsync(cityName, stationGuid);

			return Ok(result.Data);
		}
	}
}
