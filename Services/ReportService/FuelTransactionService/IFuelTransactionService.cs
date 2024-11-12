using FMSD_BE.Dtos.ReportDtos.DistributionTransactionDtos;
using FMSD_BE.Dtos.ReportDtos.TankDtos;
using FMSD_BE.Helper;

namespace FMSD_BE.Services.ReportService.DistributionTransactionService
{
    public interface IFuelTransactionService
    {
        Task<DataWithSize> GetFuelTransactions(FuelTransactionRequestViewModel input);

        List<object> ExportFuelTransactions(FuelTransactionRequestViewModel input);
    }
}
