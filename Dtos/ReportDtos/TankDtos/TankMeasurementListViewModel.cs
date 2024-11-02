namespace FMSD_BE.Dtos.ReportDtos.TankDtos
{
    public class TankMeasurementListViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public double FuelLevel { get; set; }
        public double FuelVolume { get; set; }
        public double WaterLevel { get; set; }
        public double WaterVolume { get; set; }
        public double Tcv { get; set; }
        public double Temperature {  get; set; }    

    }
}
