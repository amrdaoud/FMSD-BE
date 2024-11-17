using Azure.Core;
using FMSD_BE.Data;
using FMSD_BE.Dtos.ReportDtos.AlarmDtos;
using FMSD_BE.Dtos.SharedDto;
using FMSD_BE.Helper;
using FMSD_BE.Helper.Extensions;
using FMSD_BE.Models;
using FMSD_BE.Services.SharedService;
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
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate).AddDays(1).AddSeconds(-1);
            }

            GeneralFilterModel  generalFilterModel = new GeneralFilterModel(input.SearchQuery,input.PageIndex,
                input.PageSize,input.SortActive,input.SortDirection);

            List<string> searchFields = new List<string>() { "AlarmCode" , "AlarmStatus.Status" , "User.Name" , "User.Username" , "Description" };

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

        public List<object> ExportAlarms(AlarmRequestViewModel input)
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

            var exportData = queryViewModel.Cast<object>().ToList();


            return exportData;
        }

        private IQueryable<Alarm> ExtraFilter(IQueryable<Alarm> query, DateTime? start , DateTime? end ,
            List<string> cities , List<Guid>? stationGuids , List<string>? alarmTypes)
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

