using System.ComponentModel.DataAnnotations;

namespace Ipms.Frontend.Models
{
    public class ChangePasswordViewModel : IValidatableObject
    {
        [Required]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$",
            ErrorMessage = "Password must be 8-15 chars with uppercase, lowercase, number and special character.")]
        public string? NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewPassword != ConfirmPassword)
                yield return new ValidationResult(
                    "New password and confirm password do not match.",
                    new[] { nameof(ConfirmPassword) });

            if (CurrentPassword == NewPassword)
                yield return new ValidationResult(
                    "New password must be different from current password.",
                    new[] { nameof(NewPassword) });
        }
    }
}