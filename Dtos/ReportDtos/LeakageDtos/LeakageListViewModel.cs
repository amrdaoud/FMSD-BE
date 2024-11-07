namespace FMSD_BE.Dtos.ReportDtos.LeakageDtos
{
    public class LeakageListViewModel
    {
        public string GroupingName { get;set; }
        public double? Deviation { get; set; }
        public double? Limit { get; set; }
        public string Leakage { get; set; }
        public string? LeakageType { get; set; }
        public DateTime? CreatedAt { get; set; }


    }
}
