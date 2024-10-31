using FMSD_BE.Dtos.ReportDtos.AlarmDtos;
using FMSD_BE.Dtos.SharedDto;
using FMSD_BE.Helper;

namespace FMSD_BE.Services.ReportService.AlarmService
{
    public interface IAlarmService
    {
        Task<DataWithSize> GetAlarms(AlarmRequesViewModel input);
        FileBytesModel ExportAlarms(AlarmRequesViewModel input);
    }
}
