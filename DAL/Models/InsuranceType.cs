namespace DAL.Models
{
    public class InsuranceType
    {
        public int InsuranceTypeId { get; set; }
        public string TypeName { get; set; } = null!;      
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<InsuranceScheme> Schemes { get; set; } = new List<InsuranceScheme>();
    }
}