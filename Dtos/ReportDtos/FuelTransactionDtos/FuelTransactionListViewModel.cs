namespace FMSD_BE.Dtos.ReportDtos.DistributionTransactionDtos
{
    public class FuelTransactionListViewModel
    {
        public object? GroupingName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public long Pumb { get; set; }
        public double? OrderedAmount { get;set; }
        public double? DispensedAmount { get; set; }
        public double? MeasuredAmount { get; set;}
        public string? DispensedTo { get; set; }
        public string? Status { get;set; }
        public string? UserName {  get; set; }   
        public string? RequesitionNumber { get;set; }
        public string? DriverName { get;set; }
        public string? AccompanyingPerson { get;set; }
        public string? Plate { get;set; }
        public string? DriverLicense { get;set; }    
        public string? Note { get;set; }
        public double LiterPrice { get;set; }
        public double? TotalPrice { get; set; }
        public string? OperationType { get; set; }
    }
}
