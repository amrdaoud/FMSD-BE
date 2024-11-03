using FMSD_BE.Data;
using FMSD_BE.Dtos.ReportDtos.AlarmDtos;
using FMSD_BE.Dtos.SharedDto;
using FMSD_BE.Helper;
using FMSD_BE.Helper.Extensions;
using FMSD_BE.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using OfficeOpenXml;

namespace FMSD_BE.Services.ReportService.AlarmService
{
    public class AlarmService : IAlarmService
    {
        private readonly CentralizedFmsCloneContext _dbContext;
        public AlarmService(CentralizedFmsCloneContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DataWithSize> GetAlarms(AlarmRequestViewModel input)
        {

            if(input.StartDate != null && input.EndDate != null)
            {
                input.StartDate = Utilites.convertDateToArabStandardDate((DateTime)input.StartDate);
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate);

            }

            GeneralFilterModel  generalFilterModel = new GeneralFilterModel(input.SearchQuery,input.PageIndex,
                input.PageSize,input.SortActive,input.SortDirection);

            List<string> searchFields = new List<string>() { "AlarmCode" , "AlarmStatus.Status" , "User.Name" , "User.Username" };
            var query = _dbContext.Alarms.AsQueryable();

            query = query.ApplyFiltering(generalFilterModel, searchFields);

            query = ExtraFilter(query, input.StartDate, input.EndDate, input.Cities, input.StationGuids, input.AlarmTypes);

            var queryViewModel =  query.Select(x => new AlarmListViewModel
            {
                Id = x.Id,
                AlarmType = x.Type,
                AlarmCode = x.AlarmCode,
                Description = x.Description,
                Status = x.AlarmStatus.Status,
                AcknowledgeUser = x.User.Username,
                AlarmTime = x.CreatedAt,
                InactiveTime = x.UpdatedAt,
                AcknowledgeTime = x.AcknowledgedTime

            });

            queryViewModel = queryViewModel.ApplySortingQuerable(generalFilterModel);

            var paginitionViewModel = queryViewModel.ToPagedResult(generalFilterModel);

            return new DataWithSize(paginitionViewModel.TotalCount, paginitionViewModel.Items);


        }

        public FileBytesModel ExportAlarms(AlarmRequestViewModel input)
        {
            if (input.StartDate != null && input.EndDate != null)
            {
                input.StartDate = Utilites.convertDateToArabStandardDate((DateTime)input.StartDate);
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate);

            }

            GeneralFilterModel generalFilterModel = new GeneralFilterModel(input.SearchQuery, input.PageIndex,
                input.PageSize, input.SortActive, input.SortDirection);

            List<string> searchFields = new List<string>() { "AlarmCode", "AlarmStatus.Status", "User.Name", "User.Username" };
            var query = _dbContext.Alarms.AsQueryable();

            query = query.ApplyFiltering(generalFilterModel, searchFields);

            query = ExtraFilter(query, input.StartDate, input.EndDate, input.Cities, input.StationGuids, input.AlarmTypes);

            var queryViewModel = query.Select(x => new AlarmListViewModel
            {
                Id = x.Id,
                AlarmType = x.Type,
                AlarmCode = x.AlarmCode,
                Description = x.Description,
                Status = x.AlarmStatus.Status,
                AcknowledgeUser = x.User.Username,
                AlarmTime = x.CreatedAt,
                InactiveTime = x.UpdatedAt,
                AcknowledgeTime = x.AcknowledgedTime

            }).ToList();

            queryViewModel = queryViewModel.ApplySorting(generalFilterModel);


            //-----------------------------------------------------------------------
            if (queryViewModel == null || queryViewModel.Count() == 0)
                return new FileBytesModel();

            FileBytesModel excelfile = new();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells.LoadFromCollection(queryViewModel, true);

            List<int> dateColumns = new();
            int datecolumn = 1;
            foreach (var PropertyInfo in queryViewModel.FirstOrDefault().GetType().GetProperties())
            {
                if (PropertyInfo.PropertyType == typeof(DateTime) || PropertyInfo.PropertyType == typeof(DateTime?))
                {
                    dateColumns.Add(datecolumn);
                }
                datecolumn++;
            }
            dateColumns.ForEach(item => workSheet.Column(item).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM");
            package.Save();
            excelfile.Bytes = stream.ToArray();
            stream.Position = 0;
            stream.Close();
            string excelName = $"Alarm-Report-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            excelfile.FileName = excelName;
            excelfile.ContentType = contentType;
            return excelfile;
        }

        private IQueryable<Alarm> ExtraFilter(IQueryable<Alarm> query, DateTime? start , DateTime? end ,
            List<string> cities , List<Guid>? stationGuids , List<string> alarmTypes)
        {

            if(start != null && end != null)
            {
                query = query.Where(x => x.CreatedAt >= start && x.CreatedAt <= end);
            }

            if(cities != null && cities.Count() > 0)
            {
                cities = cities.Select(s => s.ToLower()).ToList();

                query = query.Where(x => cities.Contains(x.Station.City.ToLower()));
            }

            if(stationGuids != null && stationGuids.Count() > 0)
            {
                query = query.Where(x => stationGuids.Contains(x.StationGuid));
            }

            if(alarmTypes != null && alarmTypes.Count() > 0)
            {
                alarmTypes = alarmTypes.Select(s => s.ToLower()).ToList();

                query = query.Where(x => alarmTypes.Contains(x.Type.ToLower()));
            }

            return query;
        }


    }
}

