namespace BLL.DTOs
{
    public class UnderwriterReviewDTO
    {
        public decimal PremiumAmount { get; set; }
        public string Status { get; set; } = null!; 
        public string? Remarks { get; set; }
    }
}