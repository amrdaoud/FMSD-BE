namespace FMSD_BE.Helper
{
	public class GeneralFilterModel
	{
		public string? SearchQuery { get; set; }
		public int PageIndex { get; set; }
		public int PageSize { get; set; } = 5;
		public string? SortActive { get; set; }
		public string? SortDirection { get; set; }
	}

	public class LookUpResponse
	{
		public string Name { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;
	}
}
