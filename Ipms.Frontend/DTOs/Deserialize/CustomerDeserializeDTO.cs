namespace Ipms.Frontend.DTOs.Deserialize
{
    public class CustomerDeserializeDTO
    {
        public int customerId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? token { get; set; }
        public string? message { get; set; }
        public string? Role { get; set; }
    }
}
