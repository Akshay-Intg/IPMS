using System.ComponentModel.DataAnnotations;

namespace Ipms.Frontend.Models
{
    public class AdminEditViewModel : IValidatableObject
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
        //    ErrorMessage = "Password must be 8-15 chars with uppercase, lowercase, number and special character.")]
        //public string? Password { get; set; }

        [Required, MinLength(3), MaxLength(30)]
        public string? FirstName { get; set; }

        [Required, MinLength(3), MaxLength(30)]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public string? Gender { get; set; }

        [Required]
        public string? PhoneNo { get; set; }

        [Required, MaxLength(100)]
        public string? AddressLine1 { get; set; }

        [Required, MaxLength(50)]
        public string? City { get; set; }

        [Required, MaxLength(50)]
        public string? State { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Zip code must be exactly 6 digits.")]
        public string? ZipCode { get; set; }

        [Required]
        public int RoleId { get; set; }       

        public bool IsActive { get; set; }    

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var birthDate = DateOfBirth.ToDateTime(TimeOnly.MinValue);
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate > DateTime.Today.AddYears(-age)) age--;

            if (birthDate > DateTime.Today)
                results.Add(new ValidationResult("Date of Birth cannot be in the future.", new[] { nameof(DateOfBirth) }));
            else if (age > 120)
                results.Add(new ValidationResult("Maximum age cannot exceed 120.", new[] { nameof(DateOfBirth) }));
            else if (age < 18)
                results.Add(new ValidationResult("Minimum age limit is 18.", new[] { nameof(DateOfBirth) }));

            return results;
        }
    }
}