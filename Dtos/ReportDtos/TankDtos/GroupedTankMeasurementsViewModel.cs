using FMSD_BE.Models;

namespace FMSD_BE.Dtos.ReportDtos.TankDtos
{
    public class GroupedTankMeasurementsViewModel
    {
        public string Key { get; set; }  // Group key (City, StationName, or TankName)
        public List<TankMeasurement> Measurements { get; set; }  // List of measurements
    }
}
