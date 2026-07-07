using BLL.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class RegisterDto
        
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$", ErrorMessage = "Password must be 8 to 15 chars with at least 1 uppercase and 1 lowercase letter and 1 number and one special characters!!")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

       
        [Required]
        public string PhoneNo { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [MinimumAge(18)]
        [MaximumAge(120)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string AddressLine1 { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string ZipCode { get; set; } = string.Empty;

    }
}
