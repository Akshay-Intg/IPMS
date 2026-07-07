public class CustomerDTO
{
    public int CustomerID { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNo { get; set; }
    public string? Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string? AddressLine1 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public bool? IsActive { get; set; }
    public bool IsDeleted { get; set; }
}