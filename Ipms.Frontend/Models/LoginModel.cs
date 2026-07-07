using System.ComponentModel.DataAnnotations;

namespace Ipms.Frontend.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Email field is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
