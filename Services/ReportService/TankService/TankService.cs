using FMSD_BE.Data;
using FMSD_BE.Dtos.ReportDtos.AlarmDtos;
using FMSD_BE.Dtos.ReportDtos.LeakageDtos;
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
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate).AddDays(1).AddSeconds(-1);

            }

            GeneralFilterModel generalFilterModel = new GeneralFilterModel(input.SearchQuery, input.PageIndex,
                input.PageSize, input.SortActive, input.SortDirection);

            List<string> searchFields = new List<string>() { "Tank.TankName" };
            var query = _dbContext.TankMeasurements.AsQueryable();

            query = query.ApplyFiltering(generalFilterModel, searchFields);

            query = ExtraFilter(query, input.StartDate, input.EndDate, input.Cities, input.StationGuids, input.TankGuids);
            //-----------------------------Apply Grouping----------------------------------------
            var groupedQuery = BuildDynamicGrouping(query, input);

            //--------------------------------Apply sorting-------------------------------------------

            var queryViewModel = groupedQuery.ApplySortingQuerable(generalFilterModel);

            var paginationViewModel = queryViewModel.ToPagedResult(generalFilterModel);
            return new DataWithSize(paginationViewModel.TotalCount, paginationViewModel.Items);

        }

        public List<object> ExportTankMeasurements(TankRequestViewModel input)
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
            var groupedQuery = BuildDynamicGrouping(query, input);

            //--------------------------------Apply sorting-------------------------------------------

            var queryViewModel = groupedQuery.ApplySortingQuerable(generalFilterModel);
            var exportData = queryViewModel.ToList().Cast<object>().ToList();

            return exportData;


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


        private IQueryable<TankMeasurementGroupingResponseViewModel> BuildDynamicGrouping(IQueryable<TankMeasurement> query, TankRequestViewModel input)
        {
            IQueryable<TankMeasurementGroupingResponseViewModel> groupedQuery = null;

            IQueryable<TankMeasurementDetialsViewModel> groupedFullQuery = query.Select(x => new {
                x.Tank,
                x.Tank.Station,
                x.FuelLevel,
                x.FuelVolume,
                x.WaterLevel,
                x.WaterVolume,
                x.CreatedAt,
                x.Temperature,
                x.Tcv
            }).AsEnumerable().GroupBy(x => new { x.Tank, Row = x.CreatedAt })
                                    .Select(g => new TankMeasurementDetialsViewModel
                                    {
                                        GroupingName = $"{g.Key.Tank.Station.StationName}/{g.Key.Tank.TankName}",
                                        Date = g.Max(x => x.CreatedAt),
                                        FuelLevel = g.Average(x => x.FuelLevel),
                                        FuelVolume = g.Average(x => x.FuelVolume),
                                        WaterLevel = g.Average(x => x.WaterLevel),
                                        WaterVolume = g.Average(x => x.WaterVolume),
                                        Temperature = g.Average(x => x.Temperature),
                                        Tcv = g.Average(x => x.Tcv),
                                        TankName = g.Key.Tank.TankName,
                                        StationName = g.Key.Tank.Station.StationName,
                                        CityName = g.Key.Tank.Station.City
                                    }).AsQueryable();

            //---------------------------------Tank-------------------------------

           if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())

            {
                groupedQuery = groupedFullQuery.Select(x => new {
                    x.TankName,
                    x.StationName,
                    x.CityName,
                    x.Date,
                    x.FuelLevel,
                    x.FuelVolume,
                    x.WaterLevel,
                    x.WaterVolume,
                    x.Temperature,
                    x.Tcv,
                    x.GroupingName
                }).AsEnumerable().GroupBy(x => new { x.GroupingName, Day = x.Date.Value.DayOfYear })
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                        GroupingName = $"{g.Key.GroupingName}",
                                        FuelLevel = g.Average(x => x.FuelLevel),
                                        FuelVolume = g.Average(x => x.FuelVolume),
                                        WaterLevel = g.Average(x => x.WaterLevel),
                                        WaterVolume = g.Average(x => x.WaterVolume),
                                        Temperature = g.Average(x => x.Temperature),
                                        Tcv = g.Average(x => x.Tcv),
                                        Date = new DateTime(g.Min(x => x.Date).Value.Year, 1, 1).AddDays(g.Key.Day - 1),                                      

                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = groupedFullQuery.Select(x => new {
                    x.TankName,
                    x.StationName,
                    x.CityName,
                    x.Date,
                    x.FuelLevel,
                    x.FuelVolume,
                    x.WaterLevel,
                    x.WaterVolume,
                    x.Temperature,
                    x.Tcv,
                    x.GroupingName
                }).AsEnumerable().GroupBy(x => new { x.GroupingName, Month = x.Date.Value.Month })
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                        GroupingName = $"{g.Key.GroupingName}",
                                        FuelLevel = g.Average(x => x.FuelLevel),
                                        FuelVolume = g.Average(x => x.FuelVolume),
                                        WaterLevel = g.Average(x => x.WaterLevel),
                                        WaterVolume = g.Average(x => x.WaterVolume),
                                        Temperature = g.Average(x => x.Temperature),
                                        Tcv = g.Average(x => x.Tcv),
                                        Date = new DateTime(g.Min(x => x.Date).Value.Year,g.Key.Month, 1),
                                    }).AsQueryable();
            }


            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = groupedFullQuery.Select(x => new {
                    x.TankName,
                    x.StationName,
                    x.CityName,
                    x.Date,
                    x.FuelLevel,
                    x.FuelVolume,
                    x.WaterLevel,
                    x.WaterVolume,
                    x.Temperature,
                    x.Tcv,
                    x.GroupingName
                }).AsEnumerable().GroupBy(x => new { x.GroupingName, Year = x.Date.Value.Year })
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                        GroupingName = $"{g.Key.GroupingName}",
                                        FuelLevel = g.Average(x => x.FuelLevel),
                                        FuelVolume = g.Average(x => x.FuelVolume),
                                        WaterLevel = g.Average(x => x.WaterLevel),
                                        WaterVolume = g.Average(x => x.WaterVolume),
                                        Temperature = g.Average(x => x.Temperature),
                                        Tcv = g.Average(x => x.Tcv),
                                        Date = new DateTime(g.Key.Year, 1, 1),
                                    }).AsQueryable();
            }

            //------------------------------------Station---------------------------------
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString())
            {
                groupedQuery = groupedFullQuery.Select(x => new {
                    x.TankName,
                    x.StationName,
                    x.CityName,
                    x.Date,
                    x.FuelLevel,
                    x.FuelVolume,
                    x.WaterLevel,
                    x.WaterVolume,
                    x.Temperature,
                    x.Tcv,
                    x.GroupingName
                }).AsEnumerable().GroupBy(x => new { x.StationName,x.Date })
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                        GroupingName = $"{g.Key.StationName}",
                                        FuelLevel = g.Sum(x => x.FuelLevel),
                                        FuelVolume = g.Sum(x => x.FuelVolume),
                                        WaterLevel = g.Sum(x => x.WaterLevel),
                                        WaterVolume = g.Sum(x => x.WaterVolume),
                                        Temperature = g.Average(x => x.Temperature),
                                        Tcv = g.Sum(x => x.Tcv),
                                        Date = g.Max(x => x.Date)
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())

            {
                groupedQuery = groupedFullQuery.Select(x => new {
                    x.TankName,
                    x.StationName,
                    x.CityName,
                    x.Date,
                    x.FuelLevel,
                    x.FuelVolume,
                    x.WaterLevel,
                    x.WaterVolume,
                    x.Temperature,
                    x.Tcv,
                    x.GroupingName
                }).AsEnumerable().GroupBy(x => new { x.StationName, Day = x.Date.Value.DayOfYear })
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                      
                                        GroupingName = $"{g.Key.StationName}",
                                        FuelLevel = g.Sum(x => x.FuelLevel),
                                        FuelVolume = g.Sum(x => x.FuelVolume),
                                        WaterLevel = g.Sum(x => x.WaterLevel),
                                        WaterVolume = g.Sum(x => x.WaterVolume),
                                        Temperature = g.Average(x => x.Temperature),
                                        Tcv = g.Sum(x => x.Tcv),
                                        Date = new DateTime(g.Min(x => x.Date).Value.Year, 1, 1).AddDays(g.Key.Day - 1)
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = groupedFullQuery.Select(x => new {
                    x.TankName,
                    x.StationName,
                    x.CityName,
                    x.Date,
                    x.FuelLevel,
                    x.FuelVolume,
                    x.WaterLevel,
                    x.WaterVolume,
                    x.Temperature,
                    x.Tcv,
                    x.GroupingName
                }).AsEnumerable().GroupBy(x => new { x.StationName, Month = x.Date.Value.Month })
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                        GroupingName = $"{g.Key.StationName}",
                                        FuelLevel = g.Sum(x => x.FuelLevel),
                                        FuelVolume = g.Sum(x => x.FuelVolume),
                                        WaterLevel = g.Sum(x => x.WaterLevel),
                                        WaterVolume = g.Sum(x => x.WaterVolume),
                                        Temperature = g.Average(x => x.Temperature),
                                        Tcv = g.Sum(x => x.Tcv),
                                        Date = new DateTime(g.Min(x => x.Date).Value.Year, g.Key.Month, 1)
                                    }).AsQueryable();
            }


            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = groupedFullQuery.Select(x => new {
                    x.TankName,
                    x.StationName,
                    x.CityName,
                    x.Date,
                    x.FuelLevel,
                    x.FuelVolume,
                    x.WaterLevel,
                    x.WaterVolume,
                    x.Temperature,
                    x.Tcv,
                    x.GroupingName
                }).AsEnumerable().GroupBy(x => new { x.StationName, Year = x.Date.Value.Year })
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                        GroupingName = $"{g.Key.StationName}",
                                        FuelLevel = g.Sum(x => x.FuelLevel),
                                        FuelVolume = g.Sum(x => x.FuelVolume),
                                        WaterLevel = g.Sum(x => x.WaterLevel),
                                        WaterVolume = g.Sum(x => x.WaterVolume),
                                        Temperature = g.Average(x => x.Temperature),
                                        Tcv = g.Sum(x => x.Tcv),
                                        Date = new DateTime(g.Key.Year,1, 1)
                                    }).AsQueryable();
            }

            //----------------------------------------City--------------------------------------------

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString())
            {
                groupedQuery = groupedFullQuery.Select(x => new {
                    x.TankName,
                    x.StationName,
                    x.CityName,
                    x.Date,
                    x.FuelLevel,
                    x.FuelVolume,
                    x.WaterLevel,
                    x.WaterVolume,
                    x.Temperature,
                    x.Tcv,
                    x.GroupingName
                }).AsEnumerable().GroupBy(x => new { x.CityName, Date = x.Date })
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                        GroupingName = $"{g.Key.CityName}",
                                        FuelLevel = g.Sum(x => x.FuelLevel),
                                        FuelVolume = g.Sum(x => x.FuelVolume),
                                        WaterLevel = g.Sum(x => x.WaterLevel),
                                        WaterVolume = g.Sum(x => x.WaterVolume),
                                        Temperature = g.Average(x => x.Temperature),
                                        Tcv = g.Sum(x => x.Tcv),
                                        Date = g.Key.Date
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())

            {
                groupedQuery = groupedFullQuery.Select(x => new {
                    x.TankName,
                    x.StationName,
                    x.CityName,
                    x.Date,
                    x.FuelLevel,
                    x.FuelVolume,
                    x.WaterLevel,
                    x.WaterVolume,
                    x.Temperature,
                    x.Tcv,
                    x.GroupingName
                }).AsEnumerable().GroupBy(x => new { x.CityName, Day = x.Date.Value.DayOfYear })
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                       
                                        GroupingName = $"{g.Key.CityName}",
                                        FuelLevel = g.Sum(x => x.FuelLevel),
                                        FuelVolume = g.Sum(x => x.FuelVolume),
                                        WaterLevel = g.Sum(x => x.WaterLevel),
                                        WaterVolume = g.Sum(x => x.WaterVolume),
                                        Temperature = g.Average(x => x.Temperature),
                                        Tcv = g.Sum(x => x.Tcv),
                                        Date = new DateTime(g.Min(x => x.Date).Value.Year, 1, 1).AddDays(g.Key.Day - 1)
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = groupedFullQuery.Select(x => new {
                    x.TankName,
                    x.StationName,
                    x.CityName,
                    x.Date,
                    x.FuelLevel,
                    x.FuelVolume,
                    x.WaterLevel,
                    x.WaterVolume,
                    x.Temperature,
                    x.Tcv,
                    x.GroupingName
                }).AsEnumerable().GroupBy(x => new { x.CityName, Month = x.Date.Value.Month })
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                        GroupingName = $"{g.Key.CityName}",
                                        FuelLevel = g.Sum(x => x.FuelLevel),
                                        FuelVolume = g.Sum(x => x.FuelVolume),
                                        WaterLevel = g.Sum(x => x.WaterLevel),
                                        WaterVolume = g.Sum(x => x.WaterVolume),
                                        Temperature = g.Average(x => x.Temperature),
                                        Tcv = g.Sum(x => x.Tcv),
                                        Date = new DateTime(g.Min(x => x.Date).Value.Year, g.Key.Month, 1)
                                    }).AsQueryable();
            }


            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = groupedFullQuery.Select(x => new {
                    x.TankName,
                    x.StationName,
                    x.CityName,
                    x.Date,
                    x.FuelLevel,
                    x.FuelVolume,
                    x.WaterLevel,
                    x.WaterVolume,
                    x.Temperature,
                    x.Tcv,
                    x.GroupingName
                }).AsEnumerable().GroupBy(x => new { x.CityName, Year = x.Date.Value.Year })
                                    .Select(g => new TankMeasurementGroupingResponseViewModel
                                    {
                                        GroupingName = $"{g.Key.CityName}",
                                        FuelLevel = g.Sum(x => x.FuelLevel),
                                        FuelVolume = g.Sum(x => x.FuelVolume),
                                        WaterLevel = g.Sum(x => x.WaterLevel),
                                        WaterVolume = g.Sum(x => x.WaterVolume),
                                        Temperature = g.Average(x => x.Temperature),
                                        Tcv = g.Sum(x => x.Tcv),
                                        Date = new DateTime(g.Key.Year,1, 1)
                                    }).AsQueryable();
            }

            //----------------------------------------Each---------------------------------------------
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = groupedFullQuery.GroupBy(x => x.Date.Value.Year)
                    .Select(g => new TankMeasurementGroupingResponseViewModel
                    {
                        GroupingName = g.Min(x => x.Date).Value.Year + "-01-01",
                        FuelLevel = g.Sum(x => x.FuelLevel),
                        FuelVolume = g.Sum(x => x.FuelVolume),
                        WaterLevel = g.Sum(x => x.WaterLevel),
                        WaterVolume = g.Sum(x => x.WaterVolume),
                        Temperature = g.Average(x => x.Temperature),
                        Tcv = g.Sum(x => x.Tcv),
                        Date = new DateTime(g.Key, 1, 1)

                    });


            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = groupedFullQuery.GroupBy(x => x.Date.Value.Month)
                    .Select(g => new TankMeasurementGroupingResponseViewModel
                    {
                       
                        GroupingName = g.Min(x => x.Date).Value.Year + g.Key+ "-01",
                        FuelLevel = g.Sum(x => x.FuelLevel),
                        FuelVolume = g.Sum(x => x.FuelVolume),
                        WaterLevel = g.Sum(x => x.WaterLevel),
                        WaterVolume = g.Sum(x => x.WaterVolume),
                        Temperature = g.Average(x => x.Temperature),
                        Tcv = g.Sum(x => x.Tcv),
                        Date = new DateTime(g.Key, 1, 1)

                    });

            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())
            {
                groupedQuery = groupedFullQuery.GroupBy(x => x.Date.Value.DayOfYear)
                    .Select(g => new TankMeasurementGroupingResponseViewModel
                    {
                        GroupingName = new DateTime(g.Min(x => x.Date).Value.Year, 1, 1).AddDays(g.Key - 1).ToString(),
                        FuelLevel = g.Sum(x => x.FuelLevel),
                        FuelVolume = g.Sum(x => x.FuelVolume),
                        WaterLevel = g.Sum(x => x.WaterLevel),
                        WaterVolume = g.Sum(x => x.WaterVolume),
                        Temperature = g.Average(x => x.Temperature),
                        Tcv = g.Sum(x => x.Tcv),
                        Date = new DateTime(g.Key, 1, 1)

                    });

            }
            return groupedQuery != null ? groupedQuery : groupedFullQuery.Select(x => new TankMeasurementGroupingResponseViewModel
            {
                GroupingName = x.GroupingName,
                FuelLevel = x.FuelLevel,
                FuelVolume = x.FuelVolume,
                WaterLevel = x.WaterLevel,
                WaterVolume = x.WaterVolume,
                Temperature = x.Temperature,
                Tcv = x.Tcv,
                Date = x.Date
            });
        }

    }
}
