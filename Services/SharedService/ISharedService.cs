using FMSD_BE.Dtos.ReportDtos.AlarmDtos;
using FMSD_BE.Dtos.SharedDto;

namespace FMSD_BE.Services.SharedService
{
    public interface ISharedService
    {
        FileBytesModel ExportDynamicDataToExcel(List<object> input, string exportName);

    }
}
