using FMSD_BE.Dtos.ReportDtos.DistributionTransactionDtos;
using FMSD_BE.Dtos.ReportDtos.TransactionDetailDtos;
using FMSD_BE.Helper;

namespace FMSD_BE.Services.ReportService.TransactionDetailService
{
    public interface ITransactionDetailService
    {
        Task<DataWithSize> GetTransactionDetails(TransactionDetailRequestViewModel input);

        List<object> ExportTransactionDetails(TransactionDetailRequestViewModel input);
    }
}
