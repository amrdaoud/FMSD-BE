using FMSD_BE.Data;
using FMSD_BE.Dtos.ReportDtos.LeakageDtos;
using FMSD_BE.Helper;
using FMSD_BE.Helper.Constants.Enums;
using FMSD_BE.Helper.Extensions;
using FMSD_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FMSD_BE.Services.ReportService.LeakageSrvice
{
    public class LeakageService : ILeakageService
    {
        private readonly CentralizedFmsCloneContext _dbContext;
        public LeakageService(CentralizedFmsCloneContext dbContext)
        {
            _dbContext = dbContext;
        }
       

        public async Task<DataWithSize> GetLeakages(LeakageRequestViewModel input)
        {
            if (input.StartDate != null && input.EndDate != null)
            {
                input.StartDate = Utilites.convertDateToArabStandardDate((DateTime)input.StartDate);
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate);

            }

            GeneralFilterModel generalFilterModel = new GeneralFilterModel(input.SearchQuery, input.PageIndex,
                input.PageSize, input.SortActive, input.SortDirection);

            List<string> searchFields = new List<string>() { };

            var query = _dbContext.Leakages.AsQueryable();

            query = query.ApplyFiltering(generalFilterModel, searchFields);

            query = ExtraFilter(query, input.StartDate, input.EndDate, input.Cities, input.StationGuids, input.TankGuids, input.LeakageTypes);

            //-----------------------------Apply Grouping----------------------------------------
            var groupedQuery = BuildDynamicGrouping(query, input);

            //--------------------------------Apply sorting-------------------------------------------

            var queryViewModel = groupedQuery.ApplySortingQuerable(generalFilterModel);

            var paginationViewModel = queryViewModel.ToPagedResult(generalFilterModel);
            return new DataWithSize(paginationViewModel.TotalCount, paginationViewModel.Items);

        }

        public List<object> ExportLeakages(LeakageRequestViewModel input)
        {
            if (input.StartDate != null && input.EndDate != null)
            {
                input.StartDate = Utilites.convertDateToArabStandardDate((DateTime)input.StartDate);
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate);

            }

            GeneralFilterModel generalFilterModel = new GeneralFilterModel(input.SearchQuery, input.PageIndex,
                input.PageSize, input.SortActive, input.SortDirection);

            List<string> searchFields = new List<string>() { };

            var query = _dbContext.Leakages.AsQueryable();

            query = query.ApplyFiltering(generalFilterModel, searchFields);

            query = ExtraFilter(query, input.StartDate, input.EndDate, input.Cities, input.StationGuids, input.TankGuids, input.LeakageTypes);

            //-----------------------------Apply Grouping----------------------------------------
            var groupedQuery = BuildDynamicGrouping(query, input);

            //--------------------------------Apply sorting-------------------------------------------

            var queryViewModel = groupedQuery.ApplySortingQuerable(generalFilterModel);
            var exportData = queryViewModel.ToList().Cast<object>().ToList();

            return exportData;
        }

        private IQueryable<object> BuildDynamicGrouping(IQueryable<Leakage> query, LeakageRequestViewModel input)
        {
            IQueryable<LeakageListViewModel> groupedQuery = null;

            //---------------------------------Tank-------------------------------
            if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => new { x.Tank, Hour = x.CreatedAt.Value.Hour })
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Key.Tank.Station.StationName}/{g.Key.Tank.TankName} - {g.Key.Hour:00}:00",
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())

            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => new { x.Tank, Day = x.CreatedAt.Value.DayOfYear })
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Key.Tank.Station.StationName}/{g.Key.Tank.TankName}-{new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1)
                                                        .AddDays(g.Key.Day - 1).ToString("yyyy-MM-dd")}",
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => new { x.Tank, Month = x.CreatedAt.Value.Month })
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Key.Tank.Station.StationName}/{g.Key.Tank.TankName}-{g.Min(x => x.CreatedAt).Value.Year}-{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month)}-01",
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null
                                    }).AsQueryable();
            }


            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => new { x.Tank, Year = x.CreatedAt.Value.Year })
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Key.Tank.Station.StationName}/{g.Key.Tank.TankName}-{g.Key.Year}-01-01",
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null
                                    }).AsQueryable();
            }

            //------------------------------------Station---------------------------------
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => new { x.Tank.Station, Hour = x.CreatedAt.Value.Hour })
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Key.Station.StationName} - {g.Key.Hour:00}:00",
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())

            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => new { x.Tank.Station, Day = x.CreatedAt.Value.DayOfYear })
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Key.Station.StationName} - " +
                                        $" {new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1).AddDays(g.Key.Day - 1).ToString("yyyy-MM-dd")}",

                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => new { x.Tank.Station, Month = x.CreatedAt.Value.Month })
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Key.Station.StationName} - {g.Min(x => x.CreatedAt).Value.Year}-{g.Key.Month:D2}-01",
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null
                                    }).AsQueryable();
            }


            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => new { x.Tank.Station, Year = x.CreatedAt.Value.Year })
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Key.Station.StationName} - {g.Key.Year}-01-01",
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null
                                    }).AsQueryable();
            }

            //----------------------------------------City--------------------------------------------

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
              input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    City = x.Tank.Station.City.ToLower(),
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => new { x.City, Hour = x.CreatedAt.Value.Hour })
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Key.City} - {g.Key.Hour:00}:00",
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())

            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    City = x.Tank.Station.City.ToLower(),
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => new { x.City, Day = x.CreatedAt.Value.DayOfYear })
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Key.City} -" +
                                        $" {new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1).AddDays(g.Key.Day - 1).ToString("yyyy-MM-dd")}",

                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    City = x.Tank.Station.City.ToLower(),
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => new { x.City, Month = x.CreatedAt.Value.Month })
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Key.City} - {g.Min(x => x.CreatedAt).Value.Year}-{g.Key.Month:D2}-01",
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null
                                    }).AsQueryable();
            }


            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    City = x.Tank.Station.City.ToLower(),
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => new { x.City, Year = x.CreatedAt.Value.Year })
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Key.City} - {g.Key.Year}-01-01",
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null
                                    }).AsQueryable();
            }

            //----------------------------------------Each---------------------------------------------
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.Hour)
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = $"{g.Min(x => x.CreatedAt):yyyy-MM-dd HH}:00",
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null

                                    });
            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.DayOfYear)
                    .Select(g => new LeakageListViewModel
                    {
                        GroupingName = new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1).AddDays(g.Key - 1).ToString(),
                        Deviation = g.Sum(x => x.Deviation),
                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                        LeakageType = null,
                        Limit = g.Max(x => x.Limit),
                        CreatedAt = null

                    });

            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.Month)
                    .Select(g => new LeakageListViewModel
                    {
                        GroupingName = g.Min(x => x.CreatedAt).Value.Year + "-" +
                        CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key) + "-01",
                        Deviation = g.Sum(x => x.Deviation),
                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                        LeakageType = null,
                        Limit = g.Max(x => x.Limit),
                        CreatedAt = null

                    });

            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.Year)
                    .Select(g => new LeakageListViewModel
                    {
                        GroupingName = g.Min(x => x.CreatedAt).Value.Year + "-01-01",
                        Deviation = g.Sum(x => x.Deviation),
                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                        LeakageType = null,
                        Limit = g.Max(x => x.Limit),
                        CreatedAt = null

                    });


            }
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString())
            {
                groupedQuery = query.Select(x => new {x.Tank,x.Tank.Station,x.Deviation,x.Leakage1,x.LeakageType,
                x.Limit,x.CreatedAt}).AsEnumerable()
                    .GroupBy(x => new { x.Tank , x.Tank.Station } )
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = g.Key.Station.StationName + "/" + g.Key.Tank.TankName,
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null

                                    }).AsQueryable();
            }
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => x.Tank.Station)
                                   .Select(g => new LeakageListViewModel
                                   {
                                       GroupingName = g.Key.StationName,
                                       Deviation = g.Sum(x => x.Deviation),
                                       Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                       LeakageType = null,
                                       Limit = g.Max(x => x.Limit),
                                       CreatedAt = null

                                   }).AsQueryable();
            }
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Tank,
                    x.Tank.Station,
                    x.Deviation,
                    x.Leakage1,
                    x.LeakageType,
                    x.Limit,
                    x.CreatedAt
                }).AsEnumerable().GroupBy(x => x.Tank.Station.City.ToLower())
                                    .Select(g => new LeakageListViewModel
                                    {
                                        GroupingName = g.Key,
                                        Deviation = g.Sum(x => x.Deviation),
                                        Leakage = Math.Abs((decimal)g.Sum(x => x.Deviation)) > (decimal)g.Max(x => x.Limit) ? "Yes" : "No",
                                        LeakageType = null,
                                        Limit = g.Max(x => x.Limit),
                                        CreatedAt = null

                                    }).AsQueryable();
            }
            return groupedQuery != null ? groupedQuery : query.Select(x => new LeakageListViewModel
            {
                GroupingName = x.Tank.TankName,
                Deviation = x.Deviation,
                Leakage = x.Leakage1,
                LeakageType = x.LeakageType,
                Limit = x.Limit,
                CreatedAt = x.CreatedAt
            });
        }

        private IQueryable<Leakage> ExtraFilter(IQueryable<Leakage> query, DateTime? start,
          DateTime? end,
         List<string> cities, List<Guid>? stationGuids, List<string> TankGuids, List<string> LeakageTypes)
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
                query = query.Where(x => stationGuids.Contains(x.Tank.StationGuid));
            }

            if (TankGuids != null && TankGuids.Count() > 0)
            {

                query = query.Where(x => TankGuids.Contains(x.TankGuid.ToString()));
            }

            if ( LeakageTypes != null && LeakageTypes.Count() > 0)
            {
                query = query.Where(x => LeakageTypes.Contains(x.LeakageType));

            }

            return query;
        }

    }
}
