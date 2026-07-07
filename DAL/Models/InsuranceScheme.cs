using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class InsuranceScheme
{
    public int SchemeId { get; set; }

    public string SchemeName { get; set; } = null!;

    public string? Description { get; set; }

    public int MinAge { get; set; }

    public int MaxAge { get; set; }

    public decimal MinAmount { get; set; }

    public decimal MaxAmount { get; set; }

    public decimal ProfitRatio { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();
    public int? InsuranceTypeId { get; set; }                          
    public virtual InsuranceType? InsuranceType { get; set; }
}
