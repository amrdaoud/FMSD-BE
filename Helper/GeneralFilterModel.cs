namespace FMSD_BE.Helper
{
	public class GeneralFilterModel
	{
		public string? SearchQuery { get; set; } = string.Empty;
		public int PageIndex { get; set; } = 0;
		public int PageSize { get; set; } = 5;
		public string? SortActive { get; set; } = string.Empty ;
		public string? SortDirection { get; set; } = "asc";

		public GeneralFilterModel() { }

		public GeneralFilterModel(string? searchQuery , int pageIndex , int pageSize , string? sortActive,
            string? sortDirection)
		{
            SearchQuery = searchQuery;
			PageIndex = pageIndex;
			PageSize = pageSize;
			SortActive = sortActive;
			SortDirection = sortDirection;

		}
    }

	public class LookUpResponse
	{
		public string Name { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;
	}
}
