namespace Ipms.Frontend.Models
{
    public class AuditLogViewModel
    {
        public int AuditId { get; set; }
        public string? EntityName { get; set; }
        public int EntityId { get; set; }
        public string? ActionType { get; set; }
        public int? ActionByUserId { get; set; }
        public DateTime? ActionDate { get; set; }
    }
}