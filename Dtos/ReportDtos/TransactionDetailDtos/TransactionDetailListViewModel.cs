namespace FMSD_BE.Dtos.ReportDtos.TransactionDetailDtos
{
    public class TransactionDetailListViewModel
    {
        public string TankName { get; set; }
        public double? FuelVolumeBefore { get; set; }
        public double? FuelVolumeAfter { get; set; }
        public double? TcvBefore { get; set; }
        public double? TcvAfter { get; set; }
        public DateTime? StartedOn { get; set; } //CreatedAt
        public DateTime? EndedOn { get; set; } // UpdatedAt

    }
}
