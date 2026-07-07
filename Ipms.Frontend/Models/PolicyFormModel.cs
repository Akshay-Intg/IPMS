using System.ComponentModel.DataAnnotations;

namespace Ipms.Frontend.Models
{
    public class PolicyFormModel
    {
        public int SchemeId { get; set; }

        // Insurance Type
        [Required]
        public string? InsuranceType { get; set; }

        // Step 1 - Proposer
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(25, ErrorMessage = "Name cannot exceed 25 characters.")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Marital status is required.")]
        public string? MaritalStatus { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid contact number. Must be 10 digits.")]
        public string? Contact { get; set; }

        // ✅ Bug 4 Fixed — added [CustomValidation] attribute
        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(PolicyFormModel), nameof(ValidateAge))]
        public DateTime? DOB { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Height (feet) must be between 1 and 10.")]
        public int HeightFeet { get; set; }

        [Required]
        [Range(0, 11, ErrorMessage = "Height (inches) must be between 0 and 11.")]
        public int HeightInches { get; set; }

        // ✅ Bug 5 Fixed — renamed WeightKg → Weight to match PolicyRequestDTO
        [Required]
        [Range(1, 120, ErrorMessage = "Weight must be between 1 and 120 kg.")]
        public int Weight { get; set; }

        // Address
        [Required(ErrorMessage = "House number is required.")]
        public string? HouseNo { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Pin code is required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pin code must be 6 digits.")]
        public string? PinCode { get; set; }

        // Step 2 - Medical
        [Required]
        public bool Smokes { get; set; }
        public string? MedicalHistory { get; set; }

        // Step 3 - Documents
        public string? AadhaarDocMode { get; set; }
        public string? PanDocMode { get; set; }
        public string? AadhaarNumber { get; set; }
        public string? AadhaarBase64 { get; set; }

        // ✅ Bug 1 Fixed — renamed PanCard → PANCard to match PolicyRequestDTO
        public string? PANCard { get; set; }

        public bool DocumentsVerified { get; set; }

        // Step 4 - Nominee
        [Required(ErrorMessage = "Nominee name is required.")]
        [StringLength(30, ErrorMessage = "Nominee name cannot exceed 30 characters.")]
        public string? NomineeName { get; set; }

        [Required(ErrorMessage = "Nominee relationship is required.")]
        public string? NomineeRelationship { get; set; }

        // Life / Term only
        [Range(1, 100000000, ErrorMessage = "Sum assured must be a positive amount.")]
        public decimal? SumAssured { get; set; }

        [Range(1, 40, ErrorMessage = "Policy term must be between 1 and 40 years.")]
        public int? PolicyTerm { get; set; }

        // Group only
        [StringLength(50, ErrorMessage = "Organization name cannot exceed 50 characters.")]
        public string? OrganizationName { get; set; }

        [Range(2, 10000, ErrorMessage = "Member count must be at least 2.")]
        public int? MemberCount { get; set; }

        public string? PlanName { get; set; }

        // ✅ Bug 4 Fixed — CustomValidation method properly used
        public static ValidationResult? ValidateAge(DateTime? dob, ValidationContext context)
        {
            if (dob == null)
                return new ValidationResult("Date of birth is required.");
            if (dob > DateTime.Now.AddYears(-18))
                return new ValidationResult("Proposer must be at least 18 years old.");
            if (dob < DateTime.Now.AddYears(-100))
                return new ValidationResult("Please enter a valid date of birth.");
            return ValidationResult.Success;
        }
    }
}