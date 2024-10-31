using System.ComponentModel.DataAnnotations;

namespace FMSD_BE.CustomValidations.GeneralValidation
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly string _startDatePropertyName;
        private readonly string _endDatePropertyName;

        public DateRangeAttribute(string startDatePropertyName, string endDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
            _endDatePropertyName = endDatePropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            var endDateProperty = validationContext.ObjectType.GetProperty(_endDatePropertyName);

            if (startDateProperty == null || endDateProperty == null)
            {
                return new ValidationResult($"Unknown properties: {_startDatePropertyName} or {_endDatePropertyName}");
            }

            if (startDateProperty == null && endDateProperty == null)
            {
                return ValidationResult.Success;
            }

            var startDate = (DateTime?)startDateProperty.GetValue(validationContext.ObjectInstance);
            var endDate = (DateTime?)endDateProperty.GetValue(validationContext.ObjectInstance);

            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                return new ValidationResult($"{_startDatePropertyName} must be less than or equal to {_endDatePropertyName}.");
            }

            return ValidationResult.Success;
        }
    }
}
