using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class AdminUpdateCustomerDto
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._%+-]*@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
            ErrorMessage = "Email must start with a letter.")]
        public string Email { get; set; } = null!;
        //public string? Password { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public string PhoneNo { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public int RoleId { get; set; }        
        public bool IsActive { get; set; }     
    }
}