using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public partial class Policy
    {
        public int PolicyId { get; set; }
        public string? UnderwriterRemarks { get; set; }
        public int CustomerId { get; set; }
        public int SchemeId { get; set; }
        public int? BrokerId { get; set; }
        public string? InsuranceType { get; set; }
        public string? FullName { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Gender { get; set; }
        public string? PANCard { get; set; }
        public DateTime? DOB { get; set; }
        public int HeightFeet { get; set; }
        public int HeightInches { get; set; }
        public decimal Weight { get; set; }
        public string? HouseNo { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public bool Smokes { get; set; }
        public string? MedicalHistory { get; set; }
        public string? NomineeName { get; set; }
        public string? NomineeRelationship { get; set; }
        public int? PolicyTerm { get; set; }
        public string? OrganizationName { get; set; }
        public int? MemberCount { get; set; }
        public string? PolicyNumber { get; set; }

        // ── Already existing fields (keep these) ──
        public decimal SumAssured { get; set; }
        public decimal PremiumAmount { get; set; }
        public string? PolicyStatus { get; set; }
        public DateTime? PolicyDate { get; set; }
        public DateTime? MaturityDate { get; set; }

        // Navigation properties
        public virtual Customer? Customer { get; set; }
        public virtual InsuranceScheme? Scheme { get; set; }
        public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}