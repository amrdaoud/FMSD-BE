using FMSD_BE.Data;
using FMSD_BE.Dtos.ReportDtos.AlarmDtos;
using FMSD_BE.Dtos.ReportDtos.CalibrationDetailDtos;
using FMSD_BE.Helper;
using FMSD_BE.Helper.Extensions;
using FMSD_BE.Models;

namespace FMSD_BE.Services.ReportService.CalibrationDetailService
{
    public class CalibrationDetailService : ICalibrationDetailService
    {
        private readonly CentralizedFmsCloneContext _dbContext;
        public CalibrationDetailService(CentralizedFmsCloneContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<DataWithSize> GetCalibrationDetails(CalibrationDetailRequest input)
        {
            if (input.StartDate != null && input.EndDate != null)
            {
                input.StartDate = Utilites.convertDateToArabStandardDate((DateTime)input.StartDate);
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate);

            }

            GeneralFilterModel generalFilterModel = new GeneralFilterModel(input.SearchQuery, input.PageIndex,
                input.PageSize, input.SortActive, input.SortDirection);

            List<string> searchFields = new List<string>() { "Tank.TankName" };

            var query = _dbContext.CalibrationDetails.AsQueryable();

            query = query.ApplyFiltering(generalFilterModel, searchFields);

            query = ExtraFilter(query, input.StartDate, input.EndDate, input.Cities, input.stationGuids, input.TankGuids);

            var queryViewModel = query.Select(x => new CalibrationDetailListViewModel
            {
                Id = x.Id,
                TankName = x.Tank.TankName,
                FuelBefore = x.FuelBefore,
                FuelAfter = x.FuelAfter,
                TcvBefore = x.TcvBefore,
                TcvAfter = x.TcvAfter,
                StartedOn = x.CreatedAt,
                EndedOn = x.UpdatedAt,

            });

            queryViewModel = queryViewModel.ApplySortingQuerable(generalFilterModel);

            var paginitionViewModel = queryViewModel.ToPagedResult(generalFilterModel);

            return new DataWithSize(paginitionViewModel.TotalCount, paginitionViewModel.Items);
        }
        public List<object> ExportCalibrationDetails(CalibrationDetailRequest input)
        {
            if (input.StartDate != null && input.EndDate != null)
            {
                input.StartDate = Utilites.convertDateToArabStandardDate((DateTime)input.StartDate);
                input.EndDate = Utilites.convertDateToArabStandardDate((DateTime)input.EndDate);

            }

            GeneralFilterModel generalFilterModel = new GeneralFilterModel(input.SearchQuery, input.PageIndex,
                input.PageSize, input.SortActive, input.SortDirection);

            List<string> searchFields = new List<string>() { "Tank.TankName" };

            var query = _dbContext.CalibrationDetails.AsQueryable();

            query = query.ApplyFiltering(generalFilterModel, searchFields);

            query = ExtraFilter(query, input.StartDate, input.EndDate, input.Cities, input.stationGuids, input.TankGuids);

            var queryViewModel = query.Select(x => new CalibrationDetailListViewModel
            {
                Id = x.Id,
                TankName = x.Tank.TankName,
                FuelBefore = x.FuelBefore,
                FuelAfter = x.FuelAfter,
                TcvBefore = x.TcvBefore,
                TcvAfter = x.TcvAfter,
                StartedOn = x.CreatedAt,
                EndedOn = x.UpdatedAt,

            });

            queryViewModel = queryViewModel.ApplySortingQuerable(generalFilterModel);

            var exportData = queryViewModel.ToList().Cast<object>().ToList();

            return exportData;
        }

        private IQueryable<CalibrationDetail> ExtraFilter(IQueryable<CalibrationDetail> query, DateTime? start,
         DateTime? end,
        List<string> cities, List<Guid>? stationGuids, List<Guid> TankGuids)
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

                query = query.Where(x => TankGuids.Contains(x.TankGuid));
            }


            return query;
        }

    }
}
