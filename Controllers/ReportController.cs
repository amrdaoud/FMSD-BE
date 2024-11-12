using FMSD_BE.Dtos.ReportDtos.AlarmDtos;
using FMSD_BE.Dtos.ReportDtos.CalibrationDetailDtos;
using FMSD_BE.Dtos.ReportDtos.CalibrationDtos;
using FMSD_BE.Dtos.ReportDtos.DistributionTransactionDtos;
using FMSD_BE.Dtos.ReportDtos.LeakageDtos;
using FMSD_BE.Dtos.ReportDtos.TankDtos;
using FMSD_BE.Dtos.ReportDtos.TransactionDetailDtos;
using FMSD_BE.Services.ReportService.AlarmService;
using FMSD_BE.Services.ReportService.CalibrationDetailService;
using FMSD_BE.Services.ReportService.CalibrationService;
using FMSD_BE.Services.ReportService.DistributionTransactionService;
using FMSD_BE.Services.ReportService.LeakageSrvice;
using FMSD_BE.Services.ReportService.TankService;
using FMSD_BE.Services.ReportService.TransactionDetailService;
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
        private readonly IFuelTransactionService _fuelTransactionService;
        private readonly ITransactionDetailService _transactionDetailService;
        private readonly ILeakageService _leakageService;
        private readonly ICalibrationService _calibrationService;
        private readonly ICalibrationDetailService _calibrationDetailService;

        public ReportController(IAlarmService alarmService , ITankService tankService, 
            ISharedService sharedService,
           ILeakageService leakageService, IFuelTransactionService fuelTransactionService,
            ITransactionDetailService transactionDetailService,
            ICalibrationService calibrationService ,
            ICalibrationDetailService calibrationDetailService)
        {
            _alarmService = alarmService;
            _tankService = tankService;
            _sharedService = sharedService;
            _fuelTransactionService = fuelTransactionService;
            _transactionDetailService = transactionDetailService;      
            _leakageService = leakageService;
            _calibrationService = calibrationService;
            _calibrationDetailService = calibrationDetailService;
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

        [HttpPost("GetFuelTransactions")]
        public async Task<ActionResult> GetDistributionTransactions(FuelTransactionRequestViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _fuelTransactionService.GetFuelTransactions(input);
            return Ok(response);
        }

        [HttpPost("ExportFuelTransactions")]
        public IActionResult ExportDistributionTransactions(FuelTransactionRequestViewModel input)
        {
            var result = _fuelTransactionService.ExportFuelTransactions(input);

            var fileResult = _sharedService.ExportDynamicDataToExcel(result, "DistributionTransactions");

            if (fileResult.Bytes == null || fileResult.Bytes.Count() == 0)
                return BadRequest(new { message = "No Data To Export." });

            return File(fileResult.Bytes, fileResult.ContentType, fileResult.FileName);
        }


        [HttpPost("GetTransactionDetails")]
        public async Task<ActionResult> GetTransactionDetails(TransactionDetailRequestViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _transactionDetailService.GetTransactionDetails(input);
            return Ok(response);
        }

        [HttpPost("ExportTransactionDetails")]
        public IActionResult ExportTransactionDetails(TransactionDetailRequestViewModel input)
        {
            var result = _transactionDetailService.ExportTransactionDetails(input);

            var fileResult = _sharedService.ExportDynamicDataToExcel(result, "DistributionTransactions");

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


        [HttpPost("GetCalibrations")]
        public async Task<ActionResult> GetCalibrations(CalibrationRequestViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _calibrationService.GetCalibrations(input);
            return Ok(response);
        }


        [HttpPost("ExportCalibrations")]
        public IActionResult ExportCalibrations(CalibrationRequestViewModel input)
        {
            var result = _calibrationService.ExportCalibrations(input);

            var fileResult = _sharedService.ExportDynamicDataToExcel(result, "Calibrations");

            if (fileResult.Bytes == null || fileResult.Bytes.Count() == 0)
                return BadRequest(new { message = "No Data To Export." });

            return File(fileResult.Bytes, fileResult.ContentType, fileResult.FileName);
        }


        [HttpPost("GetCalibrationDetails")]
        public async Task<ActionResult> GetCalibrationDetails(CalibrationDetailRequest input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _calibrationDetailService.GetCalibrationDetails(input);
            return Ok(response);
        }


        [HttpPost("ExportCalibrationDetails")]
        public IActionResult ExportCalibrations(CalibrationDetailRequest input)
        {
            var result = _calibrationDetailService.ExportCalibrationDetails(input);

            var fileResult = _sharedService.ExportDynamicDataToExcel(result, "Calibration-Details");

            if (fileResult.Bytes == null || fileResult.Bytes.Count() == 0)
                return BadRequest(new { message = "No Data To Export." });

            return File(fileResult.Bytes, fileResult.ContentType, fileResult.FileName);
        }


    }
}
