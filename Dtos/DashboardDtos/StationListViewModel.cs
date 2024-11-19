namespace FMSD_BE.Dtos.DashboardDtos
{
	public class StationListViewModel
	{
		public Guid Guid { get; set; }
		public long? Id { get; set; }
		public string StationName { get; set; } = null!;
		public long TankNumber { get; set; }
		public long PumpNumber { get; set; }
		public string City { get; set; } = null!;
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public string? StationType { get; set; }
	}
}
