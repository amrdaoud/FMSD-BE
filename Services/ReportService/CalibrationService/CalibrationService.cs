using FMSD_BE.Data;
using FMSD_BE.Dtos.ReportDtos.CalibrationDtos;
using FMSD_BE.Dtos.ReportDtos.LeakageDtos;
using FMSD_BE.Helper;
using FMSD_BE.Helper.Constants.Enums;
using FMSD_BE.Helper.Extensions;
using FMSD_BE.Models;
using System.Globalization;
using System.Linq;

namespace FMSD_BE.Services.ReportService.CalibrationService
{
    public class CalibrationService : ICalibrationService
    {
        private readonly CentralizedFmsCloneContext _dbContext;
        public CalibrationService(CentralizedFmsCloneContext dbContext)
        {
            _dbContext = dbContext;
        }

       
        public async Task<DataWithSize> GetCalibrations(CalibrationRequestViewModel input)
        {
            if (input.StartDate != null && input.EndDate != null)
            {
                input.StartDate = Utilites.convertDateToArabStandardDate((DateTime)input.StartDate);
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate);

            }

            GeneralFilterModel generalFilterModel = new GeneralFilterModel(input.SearchQuery, input.PageIndex,
                input.PageSize, input.SortActive, input.SortDirection);

            List<string> searchFields = new List<string>() { };

            var query = _dbContext.Calibrations.AsQueryable();

            query = query.ApplyFiltering(generalFilterModel, searchFields);

            query = ExtraFilter(query, input.StartDate, input.EndDate, input.Cities, input.stationGuids, input.TankGuids, input.PumpIds);

            //-----------------------------Apply Grouping----------------------------------------
            var groupedQuery = BuildDynamicGrouping(query, input);

            //--------------------------------Apply sorting-------------------------------------------

            var queryViewModel = groupedQuery.ApplySortingQuerable(generalFilterModel);

            var paginationViewModel = queryViewModel.ToPagedResult(generalFilterModel);
            return new DataWithSize(paginationViewModel.TotalCount, paginationViewModel.Items);

        }

        public List<object> ExportCalibrations(CalibrationRequestViewModel input)
        {
            if (input.StartDate != null && input.EndDate != null)
            {
                input.StartDate = Utilites.convertDateToArabStandardDate((DateTime)input.StartDate);
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate);

            }

            GeneralFilterModel generalFilterModel = new GeneralFilterModel(input.SearchQuery, input.PageIndex,
                input.PageSize, input.SortActive, input.SortDirection);

            List<string> searchFields = new List<string>() { };

            var query = _dbContext.Calibrations.AsQueryable();

            query = query.ApplyFiltering(generalFilterModel, searchFields);

            query = ExtraFilter(query, input.StartDate, input.EndDate, input.Cities, input.stationGuids, input.TankGuids, input.PumpIds);

            //-----------------------------Apply Grouping----------------------------------------
            var groupedQuery = BuildDynamicGrouping(query, input);

            //--------------------------------Apply sorting-------------------------------------------

            var queryViewModel = groupedQuery.ApplySortingQuerable(generalFilterModel);

            var exportData = queryViewModel.ToList().Cast<object>().ToList();

            return exportData;
        }

        private IQueryable<object> BuildDynamicGrouping(IQueryable<Calibration> query, CalibrationRequestViewModel input)
        {
            IQueryable<CalibrationListViewModel> groupedQuery = null;

            //---------------------------------Tank-------------------------------
            if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    Tank1 = x.Pump.PumpTanks.FirstOrDefault().Tank,
                    Station1 = x.Pump.PumpTanks.FirstOrDefault().Tank.Station,
                }).AsEnumerable().GroupBy(x => new { x.Tank1 ,x.Station1,Hour = x.CreatedAt.Value.Hour })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Key.Station1.StationName}/{g.Key.Tank1.TankName} - {g.Key.Hour:00}:00",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())

            {
                groupedQuery = query.Select(x => new {
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    Tank1 = x.Pump.PumpTanks.FirstOrDefault().Tank,
                    Station1 = x.Pump.PumpTanks.FirstOrDefault().Tank.Station,
                }).AsEnumerable().GroupBy(x => new { x.Tank1,x.Station1, Day = x.CreatedAt.Value.DayOfYear })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Key.Station1.StationName}/{g.Key.Tank1.TankName}-{new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1)
                                                        .AddDays(g.Key.Day - 1).ToString("yyyy-MM-dd")}",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Pump,
                    x.Pump.PumpTanks,
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    Tank1 = x.Pump.PumpTanks.FirstOrDefault().Tank,
                    Station1 = x.Pump.PumpTanks.FirstOrDefault().Tank.Station,
                }).AsEnumerable().GroupBy(x => new { x.Tank1,x.Station1 ,Month = x.CreatedAt.Value.Month })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Key.Station1.StationName}/{g.Key.Tank1.TankName}-{g.Min(x => x.CreatedAt).Value.Year}-{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month)}-01",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null
                                    }).AsQueryable();
            }


            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    Tank1 = x.Pump.PumpTanks.FirstOrDefault().Tank,
                    Station1 = x.Pump.PumpTanks.FirstOrDefault().Tank.Station,
                }).AsEnumerable().GroupBy(x => new { x.Tank1, x.Station1 ,Year = x.CreatedAt.Value.Year })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Key.Station1.StationName}/{g.Key.Tank1.TankName}-{g.Key.Year}-01-01",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null
                                    }).AsQueryable();
            }

            //------------------------------------Station---------------------------------
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    Station1 = x.Pump.PumpTanks.FirstOrDefault().Tank.Station,
                }).AsEnumerable().GroupBy(x => new { x.Station1, Hour = x.CreatedAt.Value.Hour })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Key.Station1.StationName} - {g.Key.Hour:00}:00",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())

            {
                groupedQuery = query.Select(x => new {
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    Station1 = x.Pump.PumpTanks.FirstOrDefault().Tank.Station,
                }).AsEnumerable().GroupBy(x => new { x.Station1, Day = x.CreatedAt.Value.DayOfYear })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Key.Station1.StationName} - " +
                                        $" {new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1).AddDays(g.Key.Day - 1).ToString("yyyy-MM-dd")}",

                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    Station1 = x.Pump.PumpTanks.FirstOrDefault().Tank.Station,
                }).AsEnumerable().GroupBy(x => new { x.Station1, Month = x.CreatedAt.Value.Month })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Key.Station1.StationName} - {g.Min(x => x.CreatedAt).Value.Year}-{g.Key.Month:D2}-01",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null
                                    }).AsQueryable();
            }


            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    Station1 = x.Pump.PumpTanks.FirstOrDefault().Tank.Station,
                }).AsEnumerable().GroupBy(x => new { x.Station1, Year = x.CreatedAt.Value.Year })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Key.Station1.StationName} - {g.Key.Year}-01-01",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null
                                    }).AsQueryable();
            }

            //----------------------------------------City--------------------------------------------

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
              input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    City = x.Pump.PumpTanks.SingleOrDefault().Tank.Station.City
                }).AsEnumerable().GroupBy(x => new { x.City, Hour = x.CreatedAt.Value.Hour })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Key.City} - {g.Key.Hour:00}:00",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())

            {
                groupedQuery = query.Select(x => new {
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    City = x.Pump.PumpTanks.SingleOrDefault().Tank.Station.City
                }).AsEnumerable().GroupBy(x => new { x.City, Day = x.CreatedAt.Value.DayOfYear })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Key.City} -" +
                                        $" {new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1).AddDays(g.Key.Day - 1).ToString("yyyy-MM-dd")}",

                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null
                                    }).AsQueryable();
            }

            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    City = x.Pump.PumpTanks.SingleOrDefault().Tank.Station.City
                }).AsEnumerable().GroupBy(x => new { x.City, Month = x.CreatedAt.Value.Month })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Key.City} - {g.Min(x => x.CreatedAt).Value.Year}-{g.Key.Month:D2}-01",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null
                                    }).AsQueryable();
            }


            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString() &&
                     input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    City = x.Pump.PumpTanks.SingleOrDefault().Tank.Station.City
                }).AsEnumerable().GroupBy(x => new { x.City, Year = x.CreatedAt.Value.Year })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Key.City} - {g.Key.Year}-01-01",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null
                                    }).AsQueryable();
            }

            //----------------------------------------Each---------------------------------------------
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Hourly.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.Hour)
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = $"{g.Min(x => x.CreatedAt):yyyy-MM-dd HH}:00",
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null

                                    });
            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Daily.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.DayOfYear)
                    .Select(g => new CalibrationListViewModel
                    {
                        GroupingName = new DateTime(g.Min(x => x.CreatedAt).Value.Year, 1, 1).AddDays(g.Key - 1).ToString(),
                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                        StartDate = null,
                        EndDate = null,
                        PumpNumber = null,
                        UserName = null,
                        Note = null

                    });

            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Monthly.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.Month)
                    .Select(g => new CalibrationListViewModel
                    {
                        GroupingName = g.Min(x => x.CreatedAt).Value.Year + "-" +
                        CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key) + "-01",
                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                        StartDate = null,
                        EndDate = null,
                        PumpNumber = null,
                        UserName = null,
                        Note = null

                    });

            }
            else if (input.TimeGroup == TankMesurementConst.TankMesurementTimeGroup.Yearly.ToString())
            {
                groupedQuery = query.GroupBy(x => x.CreatedAt.Value.Year)
                    .Select(g => new CalibrationListViewModel
                    {
                        GroupingName = g.Min(x => x.CreatedAt).Value.Year + "-01-01",
                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                        StartDate = null,
                        EndDate = null,
                        PumpNumber = null,
                        UserName = null,
                        Note = null

                    });


            }
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Tank.ToString())
            {
                groupedQuery = query.Select(x => new {
                    Tank1 = x.Pump.PumpTanks.FirstOrDefault().Tank,
                    Station1 = x.Pump.PumpTanks.FirstOrDefault().Tank.Station,
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                }).AsEnumerable()
                    .GroupBy(x => new { x.Tank1 , x.Station1 })
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = g.Key.Station1.StationName + "/" + g.Key.Tank1.TankName,
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null

                                    }).AsQueryable();
            }
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.Station.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    Station1 = x.Pump.PumpTanks.SingleOrDefault().Tank.Station
                }).AsEnumerable().GroupBy(x => x.Station1)
                                   .Select(g => new CalibrationListViewModel
                                   {
                                       GroupingName = g.Key.StationName,
                                       OrderedAmount = g.Sum(x => x.OrderedAmount),
                                       DispensedAmount = g.Sum(x => x.DispAmountPump),
                                       MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                       StartDate = null,
                                       EndDate = null,
                                       PumpNumber = null,
                                       UserName = null,
                                       Note = null

                                   }).AsQueryable();
            }
            else if (input.GroupBy == TankMesurementConst.TankMesurementGroupBy.City.ToString())
            {
                groupedQuery = query.Select(x => new {
                    x.Pump,
                    x.Pump.PumpTanks,
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.User,
                    x.OrderedAmount,
                    x.DispAmountPump,
                    x.Note,
                    x.MeasuredAmount,
                    City1 = x.Pump.PumpTanks.SingleOrDefault().Tank.Station.City
                }).AsEnumerable().GroupBy(x => x.City1.ToLower())
                                    .Select(g => new CalibrationListViewModel
                                    {
                                        GroupingName = g.Key,
                                        OrderedAmount = g.Sum(x => x.OrderedAmount),
                                        DispensedAmount = g.Sum(x => x.DispAmountPump),
                                        MeasuredAmount = g.Sum(x => x.MeasuredAmount),
                                        StartDate = null,
                                        EndDate = null,
                                        PumpNumber = null,
                                        UserName = null,
                                        Note = null

                                    }).AsQueryable();
            }
            return groupedQuery != null ? groupedQuery : query.Select(x => new CalibrationListViewModel
            {
                GroupingName = null,
                OrderedAmount = x.OrderedAmount,
                DispensedAmount = x.DispAmountPump,
                MeasuredAmount =  x.MeasuredAmount,
                StartDate = x.CreatedAt,
                EndDate = x.UpdatedAt,
                PumpNumber = null,
                UserName = x.User.Username,
                Note = x.Note
            });
        }

        private IQueryable<Calibration> ExtraFilter(IQueryable<Calibration> query, DateTime? start,
          DateTime? end,
         List<string> cities, List<Guid>? stationGuids, List<Guid> TankGuids, List<Guid> PumpIds)
        {

            if (start != null && end != null)
            {
                query = query.Where(x => x.CreatedAt >= start && x.CreatedAt <= end);
            }

            if (cities != null && cities.Count() > 0)
            {
                cities = cities.Select(s => s.ToLower()).ToList();

                query = query.Where(x => cities.Contains(x.Pump.PumpTanks.Select(y => y.Tank.Station.City.ToLower()).SingleOrDefault()));
            }

            if (stationGuids != null && stationGuids.Count() > 0)
            {
                query = query.Where(x => stationGuids.Contains(x.Pump.PumpTanks.Select(y => y.Tank.StationGuid).SingleOrDefault()));
            }

            if (TankGuids != null && TankGuids.Count() > 0)
            {

                query = query.Where(x => TankGuids.Contains(x.Pump.PumpTanks.Select(y => y.Tank.Guid).SingleOrDefault()));
            }

            if (PumpIds != null && PumpIds.Count() > 0)
            {
                query = query.Where(x => PumpIds.Contains(x.PumpGuid));

            }

            return query;
        }
    }
}
