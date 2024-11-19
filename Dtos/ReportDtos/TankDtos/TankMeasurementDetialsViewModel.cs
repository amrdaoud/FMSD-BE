namespace FMSD_BE.Dtos.ReportDtos.TankDtos
{
    public class TankMeasurementDetialsViewModel
    {
        public string GroupingName { get; set; }
        public double FuelLevel { get; set; }
        public double FuelVolume { get; set; }
        public double WaterLevel { get; set; }
        public double WaterVolume { get; set; }
        public double Tcv { get; set; }
        public double Temperature { get; set; }
        public DateTime? Date { get; set; }
        public string TankName { get; set; }
        public string StationName { get; set; }
        public string CityName { get; set; }
    }
}
