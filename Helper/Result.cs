using System.Data;

namespace FMSD_BE.Helper
{
	public class ResultWithMessage(object data, string message)//FMS1
	{
		public Object? Data { get; set; } = data;
		public string? Message { get; set; } = message;
	}

	public class DataWithSize(int dataSize, object data)
	{
		public int DataSize { get; set; } = dataSize;
		public object Data { get; set; } = data;

	}
}
