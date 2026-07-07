namespace Ipms.Frontend.Models
{
    public class InsuranceTypeViewModel
    {
        public int InsuranceTypeId { get; set; }
        public string TypeName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
