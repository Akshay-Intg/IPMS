using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ipms.Frontend.Models
{
    public class EditViewModel : IValidatableObject
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._%+-]*@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
            ErrorMessage = "Email must start with a letter.")]
        public string? Email { get; set; }

        //[DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$",
        //    ErrorMessage = "Password must be 8 to 15 chars with at least 1 uppercase, 1 lowercase, 1 number, and 1 special character.")]
        //public string? Password { get; set; } 

        [MinLength(3, ErrorMessage = "Firstname must be at least 2 characters")]
        [MaxLength(30, ErrorMessage = "Firstname cannot exceed 30 characters!")]
        [Required]
        public string? FirstName { get; set; }

        [MinLength(3, ErrorMessage = "Lastname must be at least 2 characters")]
        [MaxLength(30, ErrorMessage = "Lastname cannot exceed 30 characters!")]
        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? PhoneNo { get; set; }

        [Required]
        public string? Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Address Line cannot exceed 100 characters!")]
        public string? AddressLine1 { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "City cannot exceed 50 characters!")]
        public string? City { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "State cannot exceed 50 characters!")]
        public string? State { get; set; }

        [Required]
        public string? ZipCode { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            var birthDate = DateOfBirth.ToDateTime(TimeOnly.MinValue);
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;

            if (birthDate > DateTime.Today)
                validationResults.Add(new ValidationResult(
                    "Date of Birth cannot be in the future.", new[] { nameof(DateOfBirth) }));
            else if (age > 120)
                validationResults.Add(new ValidationResult(
                    "Maximum age cannot exceed 120.", new[] { nameof(DateOfBirth) }));
            else if (age < 18)
                validationResults.Add(new ValidationResult(
                    "Minimum age limit is 18.", new[] { nameof(DateOfBirth) }));

            return validationResults;
        }
    }
}