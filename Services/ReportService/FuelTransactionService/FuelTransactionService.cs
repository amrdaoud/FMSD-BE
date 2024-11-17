using FMSD_BE.Data;
using FMSD_BE.Dtos.ReportDtos.DistributionTransactionDtos;
using FMSD_BE.Dtos.ReportDtos.TankDtos;
using FMSD_BE.Helper;
using FMSD_BE.Helper.Constants.Enums;
using FMSD_BE.Helper.Extensions;
using FMSD_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FMSD_BE.Services.ReportService.DistributionTransactionService
{
    public class FuelTransactionService : IFuelTransactionService
    {
        private readonly CentralizedFmsCloneContext _dbContext;
        public FuelTransactionService(CentralizedFmsCloneContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DataWithSize> GetFuelTransactions(FuelTransactionRequestViewModel input)
        {
            if (input.StartDate != null && input.EndDate != null)
            {
                input.StartDate = Utilites.convertDateToArabStandardDate((DateTime)input.StartDate);
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate);

            }

            GeneralFilterModel generalFilterModel = new GeneralFilterModel(input.SearchQuery, input.PageIndex,
                input.PageSize, input.SortActive, input.SortDirection);

            List<string> searchFields = new List<string>() { "StationName", "StationCity", "DetailTankName" , "DispensedTankName" };

            var query = _dbContext.FuelTransactionsVws.AsQueryable();

            query = query.ApplyFiltering(generalFilterModel, searchFields);

            query = ExtraFilter(query, input.StartDate, input.EndDate, input.Cities, input.stationNames, input.TankGuids, input.StatusIds,input.OperationTypeIds);

            //-----------------------------Apply Grouping----------------------------------------
            var groupedQuery = BuildDynamicGrouping(query,input);

            //--------------------------------Apply sorting-------------------------------------------

            var queryViewModel = groupedQuery.ApplySortingQuerable(generalFilterModel);
            // Apply pagination
            var paginationViewModel = queryViewModel.ToPagedResult(generalFilterModel);

            return new DataWithSize(paginationViewModel.TotalCount, paginationViewModel.Items);
        }
        public List<object> ExportFuelTransactions(FuelTransactionRequestViewModel input)
        {
            if (input.StartDate != null && input.EndDate != null)
            {
                input.StartDate = Utilites.convertDateToArabStandardDate((DateTime)input.StartDate);
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate);

            }

            GeneralFilterModel generalFilterModel = new GeneralFilterModel(input.SearchQuery, input.PageIndex,
                input.PageSize, input.SortActive, input.SortDirection);

            List<string> searchFields = new List<string>() { "StationName", "StationCity", "DetailTankName", "DispensedTankName" };

            var query = _dbContext.FuelTransactionsVws.AsQueryable();

            query = query.ApplyFiltering(generalFilterModel, searchFields);

            query = ExtraFilter(query, input.StartDate, input.EndDate, input.Cities, input.stationNames, input.TankGuids, input.StatusIds,input.OperationTypeIds);

            //-----------------------------Apply Grouping----------------------------------------
            var groupedQuery = BuildDynamicGrouping(query, input);

            //--------------------------------Apply sorting-------------------------------------------

            var queryViewModel = groupedQuery.ApplySortingQuerable(generalFilterModel);
            var exportData = queryViewModel.ToList().Cast<object>().ToList();

            return exportData;
        }

        private IQueryable<FuelTransactionsVw> ExtraFilter(IQueryable<FuelTransactionsVw> query, DateTime? start,
           DateTime? end, List<string> cities, List<string>? stationNames, List<string> TankGuids,
          List<long> statusIds , List<long>? optTypeIds)
        {

            if (start != null && end != null)
            {
                query = query.Where(x => x.CreatedAt >= start && x.CreatedAt <= end);
            }

            if (cities != null && cities.Count() > 0)
            {
                cities = cities.Select(s => s.ToLower()).ToList();

                query = query.Where(x => cities.Contains(x.StationCity.ToLower()));
            }

            if (stationNames != null && stationNames.Count() > 0)
            {
                query = query.Where(x => stationNames.Contains(x.StationName.ToString()));
            }

            if (TankGuids != null && TankGuids.Count() > 0)
            {

                query = query.Where(x => TankGuids.Contains(x.TankGuid.ToString()));
            }

            if(statusIds != null && statusIds.Count() > 0)
            {
                query = query.Where(x => statusIds.Contains((long)x.TransactionStatusId));

            }

            if (optTypeIds != null && optTypeIds.Count() > 0)
            {
                query = query.Where(x => optTypeIds.Contains(x.OperationTypeId));

            }

            return query;
        }

        private IQueryable<object> BuildDynamicGrouping(
    IQueryable<FuelTransactionsVw> query,
    FuelTransactionRequestViewModel input)
        {
            IQueryable<FuelTransactionListViewModel> groupedQuery = null;

            //---------------------------------Tank-------------------------------
            if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.GroupBy(x => new { x.DetailTankName,x.StationName, Hour = x.CreatedAt.Value.Hour })
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Key.StationName}/{g.Key.DetailTankName} - {g.Key.Hour:00}:00",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()
                                    });
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())

            {
                groupedQuery = query.GroupBy(x => new { x.DetailTankName,x.StationName, Day = x.CreatedAt.Value.DayOfYear })
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Key.StationName}/{g.Key.DetailTankName}-{new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1)
                                                        .AddDays(g.Key.Day - 1).ToString("yyyy-MM-dd")}",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                                    });
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.GroupBy(x => new { x.DetailTankName,x.StationName, Month = x.CreatedAt.Value.Month })
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Key.StationName}/{g.Key.DetailTankName}-{g.Min(x => x.CreatedAt).Value.Year }-{ CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month)}-01",                               
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                                    });
            }


            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.GroupBy(x => new { x.DetailTankName,x.StationName, Year = x.CreatedAt.Value.Year })
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Key.StationName}/{g.Key.DetailTankName}-{g.Key.Year}-01-01",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                                    });
            }

            //------------------------------------Station---------------------------------
           else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
               input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.GroupBy(x => new { x.StationName, Hour = x.CreatedAt.Value.Hour })
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Key.StationName} - {g.Key.Hour:00}:00",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                                    });
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())

            {
                groupedQuery = query.GroupBy(x => new { x.StationName, Day = x.CreatedAt.Value.DayOfYear })
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Key.StationName} - " +
                                        $" {new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1).AddDays(g.Key.Day - 1).ToString("yyyy-MM-dd")}",
                                                        
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                                    });
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.GroupBy(x => new { x.StationName, Month = x.CreatedAt.Value.Month })
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Key.StationName} - {g.Min(x => x.CreatedAt).Value.Year}-{g.Key.Month:D2}-01",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                                    });
            }


            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.GroupBy(x => new { x.StationName, Year = x.CreatedAt.Value.Year })
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Key.StationName} - {g.Key.Year}-01-01",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                                    });
            }

            //----------------------------------------City--------------------------------------------

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
              input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.GroupBy(x => new { x.StationCity, Hour = x.CreatedAt.Value.Hour })
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Key.StationCity} - {g.Key.Hour:00}:00",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                                    });
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())

            {
                groupedQuery = query.GroupBy(x => new { x.StationCity, Day = x.CreatedAt.Value.DayOfYear })
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Key.StationCity} -" +
                                        $" {new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1).AddDays(g.Key.Day - 1).ToString("yyyy-MM-dd")}",

                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                                    });
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.GroupBy(x => new { x.StationCity, Month = x.CreatedAt.Value.Month })
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Key.StationCity} - {g.Min(x => x.CreatedAt).Value.Year}-{g.Key.Month:D2}-01",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                                    });
            }


            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.GroupBy(x => new { x.StationCity, Year = x.CreatedAt.Value.Year })
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Key.StationCity} - {g.Key.Year}-01-01",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                                    });
            }

            //----------------------------------------Each---------------------------------------------
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.Hour)
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = $"{g.Min(x => x.CreatedAt):yyyy-MM-dd HH}:00",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),                                    
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()


                                    });
            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.DayOfYear)
                    .Select(g => new FuelTransactionListViewModel
                    {
                        GroupingName = new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1).AddDays(g.Key - 1).ToString(),
                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                        LiterPrice = g.Sum(x => x.LiterPrice),
                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()


                    });

            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.Month)
                    .Select(g => new FuelTransactionListViewModel
                    {
                        GroupingName = g.Min(x => x.CreatedAt).Value.Year + "-" +
                        CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key) + "-01",
                      
                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),                    
                        LiterPrice = g.Sum(x => x.LiterPrice),
                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()


                    });

            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.Year)
                    .Select(g => new FuelTransactionListViewModel
                    {
                        GroupingName = g.Min(x => x.CreatedAt).Value.Year + "-01-01",
                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),                       
                        LiterPrice = g.Sum(x => x.LiterPrice),
                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                    });


            }
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString())
            {
                groupedQuery = query.GroupBy(x => new {x.DetailTankName,x.StationName})
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = g.Key.StationName + "/" + g.Key.DetailTankName,
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()


                                    });
            }
            else if(input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString())
            {
                groupedQuery = query.GroupBy(x => x.StationName)
                                   .Select(g => new FuelTransactionListViewModel
                                   {
                                       GroupingName = g.Key,
                                       OrderedAmount = g.Sum(x => x.OrderedAmount),
                                       DispensedAmount = g.Sum(x => x.DispensedAmount),
                                       MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                       LiterPrice = g.Sum(x => x.LiterPrice),
                                       TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                       Pumb = g.Select(x => x.PumpGuid).Distinct().Count()

                                   });
            }
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString())
            {
                groupedQuery = query.GroupBy(x => x.StationCity)
                                    .Select(g => new FuelTransactionListViewModel
                                    {
                                        GroupingName = g.Key,
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispensedAmount),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        LiterPrice = g.Sum(x => x.LiterPrice),
                                        TotalPrice = g.Sum(x => x.LiterPrice * x.OrderedAmount),
                                        Pumb = g.Select(x => x.PumpGuid).Distinct().Count()


                                    });
            }

            return groupedQuery != null ? groupedQuery : query.Select(x => new FuelTransactionListViewModel
            {
                GroupingName = x.StationName +"/"+ x.DetailTankName,
                 Start = x.StartTime ,
                 End = x.EndTime,
                 Pumb = query.Where(y => y.TankGuid == x.TankGuid).GroupBy(t => t.PumpGuid).Count(),
                 OrderedAmount = x.OrderedAmount,
                 DispensedAmount = x.DispensedAmount,
                 MeasuredAmount = x.MeasuredAmount,
                 DispensedTo = x.DispensedTankName ,
                 Status = x.TransStatus ,
                 UserName = x.Username ,
                 RequesitionNumber = x.RequisitionNumber ,
                 DriverName = x.DriverName ,
                 AccompanyingPerson = x.AccompanyingName  ,
                 Plate =  x.TankerPlate,
                 DriverLicense =  x.DriverLicense,
                 Note = "NA",
                 LiterPrice =  x.LiterPrice,
                 TotalPrice = x.LiterPrice * x.OrderedAmount,
                 OperationType = x.Type
            });
        }

    }
}
