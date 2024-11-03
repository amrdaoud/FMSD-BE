using FMSD_BE.Data;
using FMSD_BE.Dtos.SharedDto;
using OfficeOpenXml;

namespace FMSD_BE.Services.SharedService
{
    public class SharedService : ISharedService
    {
        private readonly CentralizedFmsCloneContext _dbContext;
        public SharedService(CentralizedFmsCloneContext dbContext)
        {
            _dbContext = dbContext;
        }

        public FileBytesModel ExportDynamicDataToExcel(List<object> data, string exportName)
        {
            if (data == null || !data.Any())
                return new FileBytesModel();

            FileBytesModel excelFile = new();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                // Get properties of the first object to use as headers
                var firstItem = data.First();
                var properties = firstItem.GetType().GetProperties();

                // Create headers
                for (int i = 0; i < properties.Length; i++)
                {
                    workSheet.Cells[1, i + 1].Value = properties[i].Name;
                }

                // Populate data rows
                for (int rowIndex = 0; rowIndex < data.Count; rowIndex++)
                {
                    var item = data[rowIndex];
                    for (int colIndex = 0; colIndex < properties.Length; colIndex++)
                    {
                        var propertyValue = properties[colIndex].GetValue(item);

                        // Check if it's a DateTime and format it
                        if (propertyValue is DateTime dateValue)
                        {
                            workSheet.Cells[rowIndex + 2, colIndex + 1].Value = dateValue;
                            workSheet.Cells[rowIndex + 2, colIndex + 1].Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss AM/PM";
                        }
                        else
                        {
                            workSheet.Cells[rowIndex + 2, colIndex + 1].Value = propertyValue;
                        }
                    }
                }

                // Auto-fit columns for better readability
                workSheet.Cells.AutoFitColumns();

                // Save to stream
                package.SaveAs(stream);
                excelFile.Bytes = stream.ToArray();
            }

            // Set file name and content type
            string excelName = $"{exportName}-Report-{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";
            excelFile.FileName = excelName;
            excelFile.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return excelFile;
        }

    }
}
