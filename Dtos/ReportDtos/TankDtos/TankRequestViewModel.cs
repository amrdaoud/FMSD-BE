using FMSD_BE.CustomValidations.GeneralValidation;
using FMSD_BE.CustomValidations.ReportValidations.TankMeasurements;
using FMSD_BE.Helper;
using FMSD_BE.Helper.Constants.Enums;
using Microsoft.OpenApi.Extensions;
using System.ComponentModel;

namespace FMSD_BE.Dtos.ReportDtos.TankDtos
{
   // [DateRange("StartDate", "EndDate", ErrorMessage = "StartDate must be less than or equal to EndDate.")]
    [GroupKey("GroupBy", "TimeGroup")]

    public class TankRequestViewModel : GeneralFilterModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? Cities { get; set; }
        public List<string>? StationGuids { get; set; }
        public string? TimeGroup { get; set; } = string.Empty;
        [DefaultValue("Tank")]
        public string? GroupBy { get; set; }
        public List<string>? TankGuids { get; set; }  

    }
}
