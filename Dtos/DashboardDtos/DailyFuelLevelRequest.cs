using FMSD_BE.CustomValidations.GeneralValidation;

namespace FMSD_BE.Dtos.DashboardDtos
{
	//[DateRange("StartDate", "EndDate", ErrorMessage = "StartDate must be less than or equal to EndDate.")]

	public class DailyFuelLevelRequest
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}

	public class DailyFuelLevelResponse
	{
		public DateOnly Date { get; set; }
        public double FuelVolum { get; set; }
        public double Capacity { get; set; }
    }
}
