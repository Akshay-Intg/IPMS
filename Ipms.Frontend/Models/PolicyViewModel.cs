namespace Ipms.Frontend.Models
{
    public class PolicyViewModel
    {
        public int PolicyId { get; set; }
        public int SchemeId { get; set; }
        public string? SchemeName { get; set; }
        public string? PolicyNumber { get; set; }
        public string? InsuranceType { get; set; }
        public string? FullName { get; set; }
        public string? PolicyStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public decimal SumAssured { get; set; }
        public decimal PremiumAmount { get; set; }
        public int? PolicyTerm { get; set; }
        public DateTime? PolicyDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string? NomineeName { get; set; }
        public string? NomineeRelationship { get; set; }
    }
}