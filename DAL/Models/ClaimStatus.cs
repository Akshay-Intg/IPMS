using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ClaimStatus
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;
}
