namespace Ipms.Frontend.Models
{
    public class PolicyReviewModel
    {
        public decimal PremiumAmount { get; set; }
        public string Status { get; set; } = null!;
        public string? Remarks { get; set; }
    }
}