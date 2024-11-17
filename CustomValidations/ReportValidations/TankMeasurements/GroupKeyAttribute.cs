using System.ComponentModel.DataAnnotations;
using static FMSD_BE.Helper.Constants.Enums.TankMesurementConst;

namespace FMSD_BE.CustomValidations.ReportValidations.TankMeasurements
{
    public class GroupKeyAttribute : ValidationAttribute
    {
        private readonly string _groupByPropertyName;
        private readonly string _groupTimePropertyName;

        public GroupKeyAttribute(string groupByPropertyName, string groupTimePropertyName)
        {
            _groupByPropertyName = groupByPropertyName;
            _groupTimePropertyName = groupTimePropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var groupByPropertyName = validationContext.ObjectType.GetProperty(_groupByPropertyName);
            var groupTimePropertyName = validationContext.ObjectType.GetProperty(_groupTimePropertyName);

           

            if (groupByPropertyName == null && groupTimePropertyName == null)
            {
                return ValidationResult.Success;
            }

            var groupBy = (string?)groupByPropertyName.GetValue(validationContext.ObjectInstance);
            var groupTime = (string?)groupTimePropertyName.GetValue(validationContext.ObjectInstance);

            if (!string.IsNullOrEmpty(groupBy) &&
                (groupBy != TankMesurementGroupBy.City.ToString()
                &&  groupBy != TankMesurementGroupBy.Station.ToString()
                && groupBy != TankMesurementGroupBy.Tank.ToString() ))             
               
            {
                return new ValidationResult($"GroupBy should be :City or Station or Tank");
            }

            if (!string.IsNullOrEmpty(groupTime) &&
              (groupTime != TankMesurementTimeGroup.Yearly.ToString()
              && groupTime != TankMesurementTimeGroup.Monthly.ToString()
              && groupTime != TankMesurementTimeGroup.Daily.ToString()
              && groupTime != TankMesurementTimeGroup.Hourly.ToString()))

            {
                return new ValidationResult($"TimeGroup should be : Yearly or Monthly or Daily or  Hourly");

            }

            return ValidationResult.Success;

        }
    }
}

