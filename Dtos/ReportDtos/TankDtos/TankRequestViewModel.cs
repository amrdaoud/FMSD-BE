using FMSD_BE.Helper;

namespace FMSD_BE.Dtos.ReportDtos.TankDtos
{
    public class TankRequestViewModel : GeneralFilterModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? Cities { get; set; }
        public List<Guid>? StationGuids { get; set; }
        public string? TimeGroup { get; set; } = string.Empty;
        public string? GroupBy { get; set; } = string.Empty;
        public List<Guid>? TankGuids { get; set; }  

    }
}
