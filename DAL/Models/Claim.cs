using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Claim
{
    public int ClaimId { get; set; }

    public int PolicyId { get; set; }

    public DateTime? ClaimDate { get; set; }

    public decimal ClaimAmount { get; set; }

    public string Reason { get; set; } = null!;

    public string? ClaimStatus { get; set; }

    public virtual Policy Policy { get; set; } = null!;
}
