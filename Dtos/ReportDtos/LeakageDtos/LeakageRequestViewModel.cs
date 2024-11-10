using FMSD_BE.CustomValidations.GeneralValidation;
using FMSD_BE.Helper;
using System.ComponentModel;

namespace FMSD_BE.Dtos.ReportDtos.LeakageDtos
{
    [DateRange("StartDate", "EndDate", ErrorMessage = "StartDate must be less than or equal to EndDate.")]
    public class LeakageRequestViewModel : GeneralFilterModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? Cities { get; set; } = new List<string>();
        public List<Guid>? StationGuids { get; set; } = new List<Guid>();
        public string? TimeGroup { get; set; } = string.Empty;
        [DefaultValue("Tank")]
        public string? GroupBy { get; set; }
        public List<string>? TankGuids { get; set; } = new List<string>();
        public List<string>? LeakageTypes { get; set; } = new List<string>();

    }
}
