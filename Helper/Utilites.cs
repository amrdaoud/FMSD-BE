namespace FMSD_BE.Helper
{
	public static class Utilites
	{
		public static DateTime convertDateToArabStandardDate(DateTime dateTime)
		{
			var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
			dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);

			return dateTime;
		}
	}
}
