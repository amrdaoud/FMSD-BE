using FMSD_BE.Data;
using FMSD_BE.Dtos.ReportDtos.AlarmDtos;
using FMSD_BE.Dtos.ReportDtos.TankDtos;
using FMSD_BE.Dtos.SharedDto;
using FMSD_BE.Helper;
using FMSD_BE.Helper.Constants.Enums;
using FMSD_BE.Helper.Extensions;
using FMSD_BE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq.Dynamic.Core;

namespace FMSD_BE.Services.ReportService.TankService
{
    public class TankService : ITankService
    {
        private readonly CentralizedFmsCloneContext _dbContext;
        public TankService(CentralizedFmsCloneContext dbContext)
        {
            _dbContext = dbContext;
        }     

        public async Task<DataWithSize> GetTankMeasurements(TankRequestViewModel input)
        {
            if (input.StartDate != null && input.EndDate != null)
            {
                input.StartDate = Utilites.convertDateToArabStandardDate((DateTime)input.StartDate);
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate);

            }

            GeneralFilterModel generalFilterModel = new GeneralFilterModel(input.SearchQuery, input.PageIndex,
                input.PageSize, input.SortActive, input.SortDirection);

            List<string> searchFields = new List<string>() { "Tank.TankName" };

            var query = _dbContext.TankMeasurements.AsQueryable();

            query = query.ApplyFiltering(generalFilterModel, searchFields);

            query = ExtraFilter(query, input.StartDate, input.EndDate, input.Cities, input.StationGuids, input.TankGuids);

            //-----------------------------Apply Grouping----------------------------------------

            IQueryable<TankMeasurementGroupingResponseViewModel> groupedQuery = null;
            if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString())
            {
                groupedQuery = query.GroupBy(x => x.Tank.Station.City)
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                        GroupingName = g.Key,
                                        FuelLevel = g.Sum(x => x.FuelLevel),
                                        FuelVolume = g.Sum(x => x.FuelVolume),
                                        WaterLevel = g.Max(x => x.WaterLevel),
                                        WaterVolume = g.Max(x => x.WaterVolume),
                                        Temperature = g.Max(x => x.Temperature),
                                        Tcv = g.Sum(x => x.Tcv),
                                        
                                    });
            }
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString())
            {
                groupedQuery = query.GroupBy(x => x.Tank.Station)
                                   .Select(g => new TankMeasurementGroupingResponseViewModel
                                   {
                                       GroupingName = g.Key.StationName,
                                       FuelLevel = g.Sum(x => x.FuelLevel),
                                       FuelVolume = g.Sum(x => x.FuelVolume),
                                       WaterLevel = g.Max(x => x.WaterLevel),
                                       WaterVolume = g.Max(x => x.WaterVolume),
                                       Temperature = g.Max(x => x.Temperature),
                                       Tcv = g.Sum(x => x.Tcv),

                                   });
            }
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString())
            {
                groupedQuery = query.GroupBy(x => x.Tank)
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                        GroupingName = _dbContext.Stations.FirstOrDefault(x => x.Guid == 
                                        g.Key.StationGuid).StationName +"/"+ g.Key.TankName,
                                        FuelLevel = g.Sum(x => x.FuelLevel),
                                        FuelVolume = g.Sum(x => x.FuelVolume),
                                        WaterLevel = g.Max(x => x.WaterLevel),
                                        WaterVolume = g.Max(x => x.WaterVolume),
                                        Temperature = g.Max(x => x.Temperature),
                                        Tcv = g.Sum(x => x.Tcv),

                                    });
            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.Year)
                    .Select(g => new TankMeasurementGroupingResponseViewModel
                    {
                        GroupingName = g.Min(x => x.CreatedAt).Value.Year +"-01-01",
                        FuelLevel = g.Sum(x => x.FuelLevel),
                        FuelVolume = g.Sum(x => x.FuelVolume),
                        WaterLevel = g.Max(x => x.WaterLevel),
                        WaterVolume = g.Max(x => x.WaterVolume),
                        Temperature = g.Max(x => x.Temperature),
                        Tcv = g.Sum(x => x.Tcv),

                    });


            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.Month)
                    .Select(g => new TankMeasurementGroupingResponseViewModel
                    {
                        GroupingName = g.Min(x => x.CreatedAt).Value.Year +"-"+
                        CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key) + "-01",
                        FuelLevel = g.Sum(x => x.FuelLevel),
                        FuelVolume = g.Sum(x => x.FuelVolume),
                        WaterLevel = g.Max(x => x.WaterLevel),
                        WaterVolume = g.Max(x => x.WaterVolume),
                        Temperature = g.Max(x => x.Temperature),
                        Tcv = g.Sum(x => x.Tcv),

                    });
                                  
            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.DayOfYear)
                    .Select(g => new TankMeasurementGroupingResponseViewModel
                    {
                        GroupingName = new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1).AddDays(g.Key - 1).ToString(),
                        FuelLevel = g.Sum(x => x.FuelLevel),
                        FuelVolume = g.Sum(x => x.FuelVolume),
                        WaterLevel = g.Max(x => x.WaterLevel),
                        WaterVolume = g.Max(x => x.WaterVolume),
                        Temperature = g.Max(x => x.Temperature),
                        Tcv = g.Sum(x => x.Tcv),

                    });
                                    
            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.Hour)
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                        GroupingName = $"{g.Min(x => x.CreatedAt):yyyy-MM-dd HH}:00",
                                        FuelLevel = g.Sum(x => x.FuelLevel),
                                        FuelVolume = g.Sum(x => x.FuelVolume),
                                        WaterLevel = g.Max(x => x.WaterLevel),
                                        WaterVolume = g.Max(x => x.WaterVolume),
                                        Temperature = g.Max(x => x.Temperature),
                                        Tcv = g.Sum(x => x.Tcv),

                                    });
            }
            //--------------------------------Apply sorting-------------------------------------------

            var queryViewModel = groupedQuery.ApplySortingQuerable(generalFilterModel);
            // Apply pagination
            var paginationViewModel = queryViewModel.ToPagedResult(generalFilterModel);

            return new DataWithSize(paginationViewModel.TotalCount, paginationViewModel.Items);
        }
        public FileBytesModel ExportTankMeasurements(TankRequestViewModel input)
        {
            throw new NotImplementedException();
        }

        private IQueryable<TankMeasurement> ExtraFilter(IQueryable<TankMeasurement> query, DateTime? start,
            DateTime? end,
           List<string> cities, List<string>? stationGuids, List<string> TankGuids)
        {

            if (start != null && end != null)
            {
                query = query.Where(x => x.CreatedAt >= start && x.CreatedAt <= end);
            }

            if (cities != null && cities.Count() > 0)
            {
                cities = cities.Select(s => s.ToLower()).ToList();

                query = query.Where(x => cities.Contains(x.Tank.Station.City.ToLower()));
            }

            if (stationGuids != null && stationGuids.Count() > 0)
            {
                query = query.Where(x => stationGuids.Contains(x.Tank.StationGuid.ToString()));
            }

            if (TankGuids != null && TankGuids.Count() > 0)
            {

                query = query.Where(x => TankGuids.Contains(x.TankGuid.ToString()));
            }

            return query;
        }
    }
}
