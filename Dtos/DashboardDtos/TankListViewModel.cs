using FMSD_BE.Models;

namespace FMSD_BE.Dtos.DashboardDtos
{
	public class TankListViewModel
	{
		public string Guid { get; set; } = string.Empty;

		public long? Id { get; set; }

		public string TankName { get; set; } = string.Empty;

		public string StationGuid { get; set; } = string.Empty;

		public double DLLimit { get; set; }

		public double WLLimit { get; set; }

		public double MLLimit { get; set; }

		public double LowLimit { get; set; }

		public double LowLowLimit { get; set; }

		public double HighLimit { get; set; }

		public double HighHighLimit { get; set; }

		public double Hysteresis { get; set; }

		public double WaterHighLimit { get; set; }

		public double LogicalAddress { get; set; }

		public int PhysicalAddress { get; set; }

		public double Height { get; set; }

		public string? Description { get; set; }

		public double Capacity { get; set; }

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public long? TankStatusId { get; set; }
		public string TankStatus { get; set; } = string.Empty;
		public string StationName { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
    }
}
