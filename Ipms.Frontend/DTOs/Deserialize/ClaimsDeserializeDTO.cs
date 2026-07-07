namespace Ipms.Frontend.DTOs.Deserialize
{
    public class ClaimsDeserializeDTO
    {
        public int ClaimId { get; set; }

        public DateTime? ClaimDate { get; set; }   
        public decimal ClaimAmount { get; set; } 

        public string Reason { get; set; } = null!;
        public string? ClaimStatus { get; set; }
        public int PolicyId { get; set; }
        public string? PolicyNumber { get; set; }
    }
}
