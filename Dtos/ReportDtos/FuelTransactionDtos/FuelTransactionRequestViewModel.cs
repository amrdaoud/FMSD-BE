using FMSD_BE.CustomValidations.GeneralValidation;
using FMSD_BE.CustomValidations.ReportValidations.TankMeasurements;
using FMSD_BE.Helper;
using System.ComponentModel;

namespace FMSD_BE.Dtos.ReportDtos.DistributionTransactionDtos
{
    [DateRange("StartDate", "EndDate", ErrorMessage = "StartDate must be less than or equal to EndDate.")]
    [GroupKey("GroupBy", "TimeGroup")]
    public class FuelTransactionRequestViewModel : GeneralFilterModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? Cities { get; set; }
        public List<string>? stationNames { get; set; }
        public string? TimeGroup { get; set; } = string.Empty;
        [DefaultValue("Tank")]
        public string? GroupBy { get; set; }
        public List<string>? TankGuids { get; set; }
        public List<long>? StatusIds { get; set; }
        public List<long>? OperationTypeIds { get; set; } // filling ,  distribution ..etc

    }
}
