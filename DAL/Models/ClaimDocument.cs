using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ClaimDocument
{
    public int DocumentId { get; set; }

    public int ClaimId { get; set; }

    public string DocumentName { get; set; } = null!;

    public string DocumentType { get; set; } = null!;

    public DateTime? UploadedDate { get; set; }
}
