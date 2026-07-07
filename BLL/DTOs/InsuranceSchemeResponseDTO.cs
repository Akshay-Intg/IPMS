namespace BLL.DTOs
{
    public class InsuranceSchemeResponseDTO
    {
        public int SchemeId { get; set; }
        public int? InsuranceTypeId { get; set; }
        public string SchemeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal ProfitRatio { get; set; }
        public bool IsActive { get; set; }
        public bool status { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}