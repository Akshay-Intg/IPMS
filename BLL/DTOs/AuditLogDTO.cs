namespace BLL.DTOs
{
    public class AuditLogDTO
    {
        public int AuditId { get; set; }
        public string EntityName { get; set; } = null!;
        public int EntityId { get; set; }
        public string ActionType { get; set; } = null!;
        public int? ActionByUserId { get; set; }
        public DateTime? ActionDate { get; set; }
    }
}