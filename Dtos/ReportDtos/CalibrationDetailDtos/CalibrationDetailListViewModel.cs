namespace FMSD_BE.Dtos.ReportDtos.CalibrationDetailDtos
{
    public class CalibrationDetailListViewModel
    {
        public long Id { get; set; }
        public string TankName { get; set; }
        public double FuelBefore { get; set; }
        public double FuelAfter { get; set; }
        public double TcvBefore { get; set; }
        public double TcvAfter { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? EndedOn { get; set; }



    }
}
