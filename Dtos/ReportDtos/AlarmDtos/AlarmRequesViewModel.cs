using FMSD_BE.CustomValidations.GeneralValidation;
using FMSD_BE.Helper;

namespace FMSD_BE.Dtos.ReportDtos.AlarmDtos
{
    [DateRange("StartDate", "EndDate", ErrorMessage = "StartDate must be less than or equal to EndDate.")]
    public class AlarmRequesViewModel : GeneralFilterModel
    {
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; } 
        public List<string>? Cities { get; set; } = new List<string>();
        public List<Guid>? StationGuids { get; set; } = new List<Guid>();
        public List<string> AlarmTypes { get; set; } = new List<string>();
    }
}
