using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class AuditLog
{
    public int AuditId { get; set; }

    public string EntityName { get; set; } = null!;

    public int EntityId { get; set; }

    public string ActionType { get; set; } = null!;

    public int? ActionByUserId { get; set; }

    public DateTime? ActionDate { get; set; }
}
