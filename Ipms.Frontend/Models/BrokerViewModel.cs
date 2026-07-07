namespace Ipms.Frontend.Models
{
    public class BrokerViewModel
    {
        public int BrokerId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public string? LicenseNumber { get; set; }
        public bool IsActive { get; set; }
        public int TotalPoliciesSold { get; set; }
        public decimal TotalPremiumCollected { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class BrokerDashboardViewModel
    {
        public string? BrokerName { get; set; }
        public int TotalSold { get; set; }
        public decimal TotalPremium { get; set; }
        public List<PolicyViewModel> Policies { get; set; } = new();
    }
}