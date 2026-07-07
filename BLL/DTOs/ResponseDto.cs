using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class ResponseDto
    {
        [Required]
        public int CustomerId { get; set; } = 0;

        public bool status { get; set; } = false;
        public string message {  get; set; }= string.Empty;

    }
}
