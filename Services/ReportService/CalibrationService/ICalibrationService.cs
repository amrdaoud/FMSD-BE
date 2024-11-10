using FMSD_BE.Dtos.ReportDtos.CalibrationDtos;
using FMSD_BE.Dtos.ReportDtos.LeakageDtos;
using FMSD_BE.Helper;

namespace FMSD_BE.Services.ReportService.CalibrationService
{
    public interface ICalibrationService
    {
        Task<DataWithSize> GetCalibrations(CalibrationRequestViewModel input);

        List<object> ExportCalibrations(CalibrationRequestViewModel input);
    }
}
