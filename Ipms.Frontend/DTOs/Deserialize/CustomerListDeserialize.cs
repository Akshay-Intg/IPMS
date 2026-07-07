namespace Ipms.Frontend.DTOs.Deserialize
{
    public class CustomerListDeserialize
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int customerID { get; set; }
        public string phoneNo { get; set; }
        public string gender { get; set; }
        public DateOnly dateOfBirth { get; set; }
        public string addressLine1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public bool? isActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
