using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? PasswordHash { get; set; }

    public string? Email { get; set; } 

    public int RoleId { get; set; }

    public string? FirstName { get; set; } 

    public string? LastName { get; set; } 

    public string? PhoneNo { get; set; } 

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? AddressLine1 { get; set; }

    public string? City { get; set; } 

    public string? State { get; set; } 

    public string? ZipCode { get; set; } 

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();

    public virtual Role Role { get; set; } = null!;
    public string? OTP { get; set; }
    public DateTime? OTPExpiry { get; set; }
    public string? LicenseNumber { get; set; }
    public bool IsDeleted { get; set; } = false;
}
