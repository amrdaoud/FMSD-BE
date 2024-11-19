using FMSD_BE.Dtos.DashboardDtos;
using FMSD_BE.Services.FilterService;
using Microsoft.AspNetCore.Mvc;

namespace FMSD_BE.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FiltersController(IFilterService filterService) : ControllerBase
	{
		private readonly IFilterService _filterService = filterService;

		[HttpGet("GetAllCities")]
		public async Task<IActionResult> GetAllCities()
		{
			var result = await _filterService.GetAllCitiesAsync();

			return Ok(result.Data);
		}


		[HttpGet("GetStation")]
		public async Task<IActionResult> GetStation(string? name)
		{
			var result = await _filterService.GetAllStationsAsync(name);

			return Ok(result.Data);
		}


		[HttpGet("GetAllAlarmTypes")]
		public async Task<IActionResult> GetAllAlarmTypes()
		{
			var result = await _filterService.GetAllAlarmTypesAsync();

			return Ok(result.Data);
		}


		[HttpGet("GetTransactionStatuses")]
		public async Task<IActionResult> GetTransactionStatuses()
		{
			var result = await _filterService.GetTransactionStatusesAsync();

			return Ok(result.Data);
		}


		[HttpPost("GetAllTanks")]
		public async Task<IActionResult> GetAllTanks(GetTanksRequest request)
		{
			var result = await _filterService.GetAllTanksAsync(request);

			return Ok(result.Data);
		}
	}
}
