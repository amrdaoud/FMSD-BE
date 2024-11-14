namespace FMSD_BE.Dtos.DashboardDtos
{
	public class GetTanksRequest
	{
		public string? CityName { get; set; } = string.Empty;
		public string? StationGuid { get; set; } = string.Empty;
	}
}
