using System.Data;

namespace FMSD_BE.Helper
{
	public class ResultWithMessage(object data, string message)
	{
		public Object? Data { get; set; } = data;
		public string? Message { get; set; } = message;
	}

	public class DataWithSize(int dataSize, object data)
	{
		public int DataSize { get; set; } = dataSize;
		public object Data { get; set; } = data;
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

	public class ChartApiResponse
	{
		public List<DataSetModel> Datasets { get; set; } = [];
		public List<string> Labels { get; set; } = [];
		public List<LookUpResponse> Values { get; set; } = [];
	}
}
