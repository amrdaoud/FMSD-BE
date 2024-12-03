﻿using FMSD_BE.Services.DashboardService;
using Microsoft.AspNetCore.Mvc;

namespace FMSD_BE.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DashboardsController(IDashboardService dashboardService) : ControllerBase
	{
		private readonly IDashboardService _dashboardService = dashboardService;

		[HttpGet("CityReport")]
		public async Task<IActionResult> CityReport(string? name, bool tcv)
		{
			var result = await _dashboardService.CityReportAsync(name, tcv);

			if (!string.IsNullOrEmpty(result.Message))
				return BadRequest(new { message = result.Message });

			return Ok(result.Data);
		}

		[HttpGet("StationReport")]
		public async Task<IActionResult> StationsReport(string? name, bool tcv)
		{
			var result = await _dashboardService.StationReportAsync(name, tcv);

			if (!string.IsNullOrEmpty(result.Message))
				return BadRequest(new { message = result.Message });

			return Ok(result.Data);
		}

		[HttpGet("TankReport")]
		public async Task<IActionResult> TankReport(string? name, bool tcv)
		{
			var result = await _dashboardService.TankReportAsync(name, tcv);

			if (!string.IsNullOrEmpty(result.Message))
				return BadRequest(new { message = result.Message });

			return Ok(result.Data);
		}

		[HttpGet("TanksDailyFuelVolume")]
		public async Task<IActionResult> TanksDailyFuelVolume(DateTime startDate, DateTime endDate)
		{
			var result = await _dashboardService.TanksDailyFuelVolumeAsync(startDate, endDate);

			if (!string.IsNullOrEmpty(result.Message))
				return BadRequest(new { message = result.Message });

			return Ok(result.Data);
		}


		[HttpGet("AlarmTypesChart")]
		public async Task<IActionResult> AlarmTypesChart(DateTime startDate, DateTime endDate)
		{
			var result = await _dashboardService.AlarmTypesChartAsync(startDate, endDate);

			if (!string.IsNullOrEmpty(result.Message))
				return BadRequest(new { message = result.Message });

			return Ok(result.Data);
		}


		[HttpGet("DailyLeackageChart")]
		public async Task<IActionResult> DailyLeackageChart(string? name, DateTime startDate, DateTime endDate)
		{
			var result = await _dashboardService.DailyLeackageChartAsync(name, startDate, endDate);
			return Ok(result.Data);
		}


		[HttpGet("DailyLeackagesPerStationChart")]
		public async Task<IActionResult> DailyLeackagesPerStationChart(string? name, DateTime startDate, DateTime endDate)
		{
			var result = await _dashboardService.DailyLeackagesPerStationChartAsync(name, startDate, endDate);
			return Ok(result.Data);
		}


		[HttpGet("DailyLeackagesPerTankChart")]
		public async Task<IActionResult> DailyLeackagesPerTankChart(string? name, DateTime startDate, DateTime endDate)
		{
			var result = await _dashboardService.DailyLeackagesPerTankChartAsync(name, startDate, endDate);
			return Ok(result.Data);
		}
	}
}