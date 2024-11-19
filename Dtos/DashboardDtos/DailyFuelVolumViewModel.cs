namespace FMSD_BE.Dtos.DashboardDtos
{
	public class DailyFuelVolumViewModel
	{
		public DateOnly Date { get; set; }
		public double FuelVolum { get; set; }
		public double Capacity { get; set; }
	}
}
