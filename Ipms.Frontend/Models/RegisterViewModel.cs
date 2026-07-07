using System.ComponentModel.DataAnnotations;
namespace Ipms.Frontend.Models
{
    public class RegisterViewModel: IValidatableObject
    {
        [Required, EmailAddress]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._%+-]*@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",ErrorMessage ="Email format is not valid!")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$", ErrorMessage ="Password must be 8 to 15 chars with at least 1 uppercase and 1 lowercase letter and 1 number and one special characters!!")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword {  get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Firstname must be at least 3 characters")]
        [MaxLength(30, ErrorMessage = "Firstname cannot exceed 30 characters!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Firstname must contain only letters and spaces")]
        public string? FirstName { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Lastname must be at least 2 characters")]
        [MaxLength(30, ErrorMessage = "Lastname cannot exceed 30 characters!")]
        public string? LastName { get; set; }


        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string? PhoneNo { get; set; }

        [Required]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? AddressLine1 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City must contain letters only.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "State must contain letters only.")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Zip code is required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Zip code must be exactly 6 digits.")]
        public string? ZipCode { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            if (DateOfBirth == null)
            {
                validationResults.Add(new ValidationResult(
                "Date of birth is required.",
                new[] { nameof(DateOfBirth) }));
                return validationResults;
            }
            if (DateOfBirth is DateTime birthDate)
            {
                var age = DateTime.Today.Year - birthDate.Year;
                if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;

                if (birthDate > DateTime.Today)
                { 
                validationResults.Add(new ValidationResult(
                "The Created Date cannot be in the future.",
                new[] { nameof(DateOfBirth) }));
                }

                else if (age > 120)
                {
                    validationResults.Add(new ValidationResult(
                    "Max Age cannot exceed 120.",
                    new[] { nameof(DateOfBirth) }));
                }
                else if (age<18)
                {
                    validationResults.Add(new ValidationResult(
                    "Min Age Limit is 18.",
                    new[] { nameof(DateOfBirth) }));
                }

            }
            return validationResults;
        }
    }
}
