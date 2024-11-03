using FMSD_BE.Dtos.ReportDtos.AlarmDtos;
using FMSD_BE.Dtos.ReportDtos.TankDtos;
using FMSD_BE.Services.ReportService.AlarmService;
using FMSD_BE.Services.ReportService.TankService;
using FMSD_BE.Services.SharedService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace FMSD_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IAlarmService _alarmService;

        private readonly ITankService _tankService;
        private readonly ISharedService _sharedService;


        public ReportController(IAlarmService alarmService , ITankService tankService, ISharedService sharedService)
        {
            _alarmService = alarmService;
            _tankService = tankService;
            _sharedService = sharedService;
        }

        [HttpPost("GetAlarms")]
        public async Task<ActionResult> GetAlarms(AlarmRequestViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _alarmService.GetAlarms(input);
            return Ok(response);
        }

        [HttpPost("ExportAlarms")]
        public IActionResult ExportAlarms(AlarmRequestViewModel input)
        {
            var result = _alarmService.ExportAlarms(input);
            var fileResult = _sharedService.ExportDynamicDataToExcel(result,"Alarm");

            if (fileResult.Bytes == null || fileResult.Bytes.Count() == 0)
                return BadRequest(new { message = "No Data To Export." });

            return File(fileResult.Bytes, fileResult.ContentType, fileResult.FileName);
        }


        [HttpPost("GetTanks")]
        public async Task<ActionResult> GetTanks(TankRequestViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _tankService.GetTankMeasurements(input);
            return Ok(response);
        }

    }
}
