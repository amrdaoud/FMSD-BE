using FMSD_BE.Dtos.ReportDtos.DistributionTransactionDtos;
using FMSD_BE.Dtos.ReportDtos.TankDtos;
using FMSD_BE.Helper;

namespace FMSD_BE.Services.ReportService.DistributionTransactionService
{
    public interface IDistributionTransactionService
    {
        Task<DataWithSize> GetDistributionTransactions(DistributionTransactionRequestViewModel input);

        List<object> ExportDistributionTransactions(DistributionTransactionRequestViewModel input);
    }
}
