using System.ComponentModel.DataAnnotations;

namespace BLL.Attributes
{
    public class MaximumAgeAttribute : ValidationAttribute
    {
        private readonly int _maxage;

        public MaximumAgeAttribute(int maxage)
        {
            _maxage = maxage;
            ErrorMessage = $"Max Age Limit is {_maxage} years old.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime birthDate)
            {
                var age = DateTime.Today.Year - birthDate.Year;
                if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;

                if (age > _maxage)
                {
                    return new ValidationResult(ErrorMessage);
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid date.");
        }
    }
}
