﻿using FMSD_BE.Dtos.ReportDtos.AlarmDtos;
using FMSD_BE.Dtos.ReportDtos.LeakageDtos;
using FMSD_BE.Dtos.ReportDtos.TankDtos;
using FMSD_BE.Services.ReportService.AlarmService;
using FMSD_BE.Services.ReportService.LeakageSrvice;
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


        private readonly ILeakageService _leakageService;
        public ReportController(IAlarmService alarmService , ITankService tankService, ISharedService sharedService,
           ILeakageService leakageService)
        {
            _alarmService = alarmService;
            _tankService = tankService;
            _sharedService = sharedService;
            
            _leakageService = leakageService;
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

        [HttpPost("ExportTankMeasurements")]
        public IActionResult ExportAlarms(TankRequestViewModel input)
        {
            var result = _tankService.ExportTankMeasurements(input);

            var fileResult = _sharedService.ExportDynamicDataToExcel(result, "TankMeasurements");

            if (fileResult.Bytes == null || fileResult.Bytes.Count() == 0)
                return BadRequest(new { message = "No Data To Export." });

            return File(fileResult.Bytes, fileResult.ContentType, fileResult.FileName);
        }



























        [HttpPost("GetLeakages")]
        public async Task<ActionResult> GetLeakages(LeakageRequestViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _leakageService.GetLeakages(input);
            return Ok(response);
        }

        [HttpPost("ExportLeakages")]
        public IActionResult ExportLeakages(LeakageRequestViewModel input)
        {
            var result = _leakageService.ExportLeakages(input);

            var fileResult = _sharedService.ExportDynamicDataToExcel(result, "Leakages");

            if (fileResult.Bytes == null || fileResult.Bytes.Count() == 0)
                return BadRequest(new { message = "No Data To Export." });

            return File(fileResult.Bytes, fileResult.ContentType, fileResult.FileName);
        }

    }
}
