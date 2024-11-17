using FMSD_BE.Helper;

namespace FMSD_BE.Dtos.DashboardDtos
{
	public class ChartApiResponse
	{
		public List<DataSetModel> Datasets { get; set; } = [];
		public List<string> Labels { get; set; } = [];
		public List<LookUpResponse> Values { get; set; } = [];
		public CardValue? CardValue { get; set; }
	}
	public class DataSetModel
	{
		public List<double> Data { get; set; } = [];
		public List<string>? BackgroundColor { get; set; }
		public List<string>? BorderColor { get; set; }
		public string? Fill { get; set; }
		public string? Label { get; set; }
		public string? Stack { get; set; }
		public string? YAxisId { get; set; }
	}
	public class CardValue
	{
		//public bool? IsUp { get; set; }
		public string? BoldValue { get; set; }
		public string? BoldValueTitle { get; set; }
		public string? LightValue { get; set; }
		public string? LightValueTitle { get; set; }
		public Icon? Icon { get; set; }
	}
	public class Icon
	{
		public string Text { get; set; } = string.Empty;
		public string? Color { get; set; }
	}
}
