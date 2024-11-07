using FMSD_BE.Dtos.ReportDtos.LeakageDtos;
using FMSD_BE.Helper;

namespace FMSD_BE.Services.ReportService.LeakageSrvice
{
    public interface ILeakageService
    {
        Task<DataWithSize> GetLeakages(LeakageRequestViewModel input);

        List<object> ExportLeakages(LeakageRequestViewModel input);
    }
}
