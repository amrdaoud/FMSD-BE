using FMSD_BE.CustomValidations.GeneralValidation;

namespace FMSD_BE.Dtos.DashboardDtos
{
	[DateRange("StartDate", "EndDate", ErrorMessage = "StartDate must be less than or equal to EndDate.")]

	public class DailyFuelLevelRequest
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
