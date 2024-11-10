namespace FMSD_BE.Dtos.ReportDtos.CalibrationDtos
{
    public class CalibrationListViewModel
    {
        public string GroupingName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PumpNumber { get;set; }
        public string? UserName { get; set; }
        public double? OrderedAmount { get; set; }
        public double? DispensedAmount { get; set; }
        public double? MeasuredAmount { get; set; }
        public string? Note { get; set; }

    }
}
