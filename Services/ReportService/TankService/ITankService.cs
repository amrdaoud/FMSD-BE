using FMSD_BE.Dtos.ReportDtos.AlarmDtos;
using FMSD_BE.Dtos.ReportDtos.TankDtos;
using FMSD_BE.Dtos.SharedDto;
using FMSD_BE.Helper;

namespace FMSD_BE.Services.ReportService.TankService
{
    public interface ITankService
    {
        Task<DataWithSize> GetTankMeasurements(TankRequestViewModel input);
        FileBytesModel ExportTankMeasurements(TankRequestViewModel input);
    }
}
