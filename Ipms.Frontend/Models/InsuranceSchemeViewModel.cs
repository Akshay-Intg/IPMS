using System.ComponentModel.DataAnnotations;

namespace Ipms.Frontend.Models
{
    public class InsuranceSchemeViewModel : IValidatableObject
    {
        public int SchemeId { get; set; }
        public int? InsuranceTypeId { get; set; }

        [Required(ErrorMessage = "Scheme name is required.")]
        public string SchemeName { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

        [Required]
        [Range(18, 120, ErrorMessage = "MinAge must be between 18 and 120.")]
        public int MinAge { get; set; }

        [Required]
        [Range(18, 120, ErrorMessage = "MaxAge must be between 18 and 120.")]
        public int MaxAge { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "MinAmount must be positive.")]
        public decimal MinAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "MaxAmount must be positive.")]
        public decimal MaxAmount { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "ProfitRatio must be between 0 and 100.")]
        public decimal ProfitRatio { get; set; }

        public bool IsActive { get; set; }  

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MinAge >= MaxAge)
                yield return new ValidationResult(
                    "MinAge must be less than MaxAge.",
                    new[] { nameof(MinAge), nameof(MaxAge) });

            if (MinAmount >= MaxAmount)
                yield return new ValidationResult(
                    "MinAmount must be less than MaxAmount.",
                    new[] { nameof(MinAmount), nameof(MaxAmount) });
        }
    }
}