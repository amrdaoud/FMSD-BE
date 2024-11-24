namespace FMSD_BE.Dtos.ReportDtos.AlarmDtos
{
    public class AlarmListViewModel
    {
        public long Id { get;set; }
        public string AlarmType { get;set; } = string.Empty;
        public string AlarmCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get;set; }
        public string AcknowledgeUser { get; set; } = string.Empty;
        public DateTime? AlarmTime { get;set; }
        public DateTime? InactiveTime { get;set; }
        public DateTime? AcknowledgeTime { get; set; }
        public string StationName { get; set; }

    }
}
