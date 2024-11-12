using FMSD_BE.Dtos.ReportDtos.CalibrationDetailDtos;
using FMSD_BE.Dtos.ReportDtos.TransactionDetailDtos;
using FMSD_BE.Helper;

namespace FMSD_BE.Services.ReportService.CalibrationDetailService
{
    public interface ICalibrationDetailService
    {
        Task<DataWithSize> GetCalibrationDetails(CalibrationDetailRequest input);

        List<object> ExportCalibrationDetails(CalibrationDetailRequest input);
    }
}
