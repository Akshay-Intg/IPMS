using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int PolicyId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal Amount { get; set; }
    public string PaymentStatus { get; set; } = null!;

    public string PaymentMode { get; set; } = null!;

    public virtual Policy Policy { get; set; } = null!;
}
