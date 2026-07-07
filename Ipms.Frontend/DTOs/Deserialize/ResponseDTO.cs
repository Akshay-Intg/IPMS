using System.ComponentModel.DataAnnotations;

namespace Ipms.Frontend.DTOs.Deserialize
{
    public class ResponseDTO
    {
        [Required]
        public int CustomerId { get; set; } = 0;

        public bool status { get; set; } = false;
        public string message { get; set; } = string.Empty;
    }
}
