namespace Ipms.Frontend.Models
{
    public class PolicyUnderwriterViewModel
    {
        public int PolicyId { get; set; }
        public string? PolicyNumber { get; set; }
        public string? InsuranceType { get; set; }
        public string? SchemeName { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public DateTime? DOB { get; set; }
        public int Age { get; set; }
        public string? PANCard { get; set; }
        public int HeightFeet { get; set; }
        public int HeightInches { get; set; }
        public decimal Weight { get; set; }
        public decimal BMI { get; set; }
        public string? BMICategory { get; set; }
        public bool Smokes { get; set; }
        public string? MedicalHistory { get; set; }
        public string? NomineeName { get; set; }
        public string? NomineeRelationship { get; set; }
        public string? HouseNo { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public decimal SumAssured { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal SuggestedPremium { get; set; }
        public decimal BasePremium { get; set; }
        public decimal AgeFactor { get; set; }
        public decimal BMIFactor { get; set; }
        public decimal SmokerFactor { get; set; }
        public int? PolicyTerm { get; set; }
        public string? PolicyStatus { get; set; }
        public string? UnderwriterRemarks { get; set; }
        public DateTime? PolicyDate { get; set; }
        public DateTime? MaturityDate { get; set; }
    }
}