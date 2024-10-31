using FMSD_BE.Dtos.ReportDtos.AlarmDtos;
using FMSD_BE.Services.ReportService.AlarmService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FMSD_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IAlarmService _alarmService;

        public ReportController(IAlarmService alarmService)
        {
            _alarmService = alarmService;
        }

        [HttpPost("GetAlarms")]
        public async Task<ActionResult> GetAlarms(AlarmRequesViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _alarmService.GetAlarms(input);
            return Ok(response);
        }


    }
}
