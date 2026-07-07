namespace BLL.DTOs
{
    public class PolicyListResponseDTO
    {
        public int PolicyId { get; set; }
        public string? PolicyNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? InsuranceType { get; set; }
        public string? SchemeName { get; set; }
        public string? PolicyStatus { get; set; }
        public decimal SumAssured { get; set; }
        public decimal PremiumAmount { get; set; }
        public int? PolicyTerm { get; set; }
        public DateTime? PolicyDate { get; set; }
        public DateTime? MaturityDate { get; set; }
    }
}