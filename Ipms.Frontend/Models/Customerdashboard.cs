namespace Ipms.Frontend.Models
{
    public class Customerdashboard
    {
        public string FullName { get; set; } = "";
        public int TotalPolicies { get; set; }
        public int ActiveClaims { get; set; }
        public decimal TotalPremiumPaid { get; set; }

        public string? PolicyNumber { get; set; }
        public string? InsuranceType { get; set; }
        public string? PolicyStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal SumAssured { get; set; }
        public int? PolicyTerm { get; set; }
        public DateTime? PolicyDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string? NomineeName { get; set; }

        public List<PolicyViewModel> Policies { get; set; } = new();
    }
}