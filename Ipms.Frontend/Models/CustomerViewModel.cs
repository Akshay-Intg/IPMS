namespace Ipms.Frontend.Models
{
    public class CustomerViewModel
    {
        public int customerId { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phoneNo { get; set; } = string.Empty;
        public DateTime? dateOfBirth { get; set; }
    }
}
