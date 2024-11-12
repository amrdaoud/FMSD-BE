using FMSD_BE.Dtos.DashboardDtos;
using FMSD_BE.Services.DashboardService;
using Microsoft.AspNetCore.Mvc;

namespace FMSD_BE.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DashboardController(IDashboardService dashboardService) : ControllerBase
	{
		private readonly IDashboardService _dashboardService = dashboardService;

		[HttpGet("CityReport")]
		public async Task<IActionResult> CityReport(string? name, bool tcv)
		{
			var result = await _dashboardService.GetCityReportAsync(name, tcv);

			if (!string.IsNullOrEmpty(result.Message))
				return BadRequest(new { message = result.Message });

			return Ok(result.Data);
		}

		[HttpGet("StationReport")]
		public async Task<IActionResult> StationsReport(string? name, bool tcv)
		{
			var result = await _dashboardService.GetStationReportAsync(name, tcv);

			if (!string.IsNullOrEmpty(result.Message))
				return BadRequest(new { message = result.Message });

			return Ok(result.Data);
		}

		[HttpGet("TankReport")]
		public async Task<IActionResult> TankReport(string? name, bool tcv)
		{
			var result = await _dashboardService.GetTankReportAsync(name, tcv);

			if (!string.IsNullOrEmpty(result.Message))
				return BadRequest(new { message = result.Message });

			return Ok(result.Data);
		}

		[HttpPost("TanksDailyFuelLevel")]
		public async Task<IActionResult> DailyFuelLevel(DailyFuelLevelRequest request)
		{
			var result = await _dashboardService.TanksDailyFuelLevelAsync(request);

			if (!string.IsNullOrEmpty(result.Message))
				return BadRequest(new { message = result.Message });

			return Ok(result.Data);
		}

		[HttpGet("GetAllCities")]
		public async Task<IActionResult> GetAllCities()
		{
			var result = await _dashboardService.GetAllCitiesAsync();

			return Ok(result.Data);
		}


		[HttpGet("GetStation")]
		public async Task<IActionResult> GetStation(string? name)
		{
			var result = await _dashboardService.GetAllStationsAsync(name);

			return Ok(result.Data);
		}


		[HttpGet("GetAllAlarmTypes")]
		public async Task<IActionResult> GetAllAlarmTypes()
		{
			var result = await _dashboardService.GetAllAlarmTypesAsync();

			return Ok(result.Data);
		}


		[HttpGet("GetTransactionStatuses")]
		public async Task<IActionResult> GetTransactionStatuses()
		{
			var result = await _dashboardService.GetTransactionStatusesAsync();

			return Ok(result.Data);
		}


	}
}
