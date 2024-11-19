namespace FMSD_BE.Dtos.DashboardDtos
{
	public class DailyFuelVolumeViewModel
	{
		public DateOnly Date { get; set; }
		public double FuelVolume { get; set; }
		public double Capacity { get; set; }
	}
}
