using FMSD_BE.Services.DashboardService;
using Microsoft.AspNetCore.Mvc;

namespace FMSD_BE.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DashboardController(IDashboardService dashboardService) : ControllerBase
	{
		private readonly IDashboardService _dashboardService = dashboardService;

		[HttpGet("StationsReport")]
		public async Task<IActionResult> firstReport()
		{
			var result = await _dashboardService.GetStationsReportAsync();

			if (!string.IsNullOrEmpty(result.Message))
				return BadRequest(new { message = result.Message });

			return Ok(result.Data);
		}
	}
}
