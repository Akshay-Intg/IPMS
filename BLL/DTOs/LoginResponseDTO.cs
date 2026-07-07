using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class LoginResponseDTO
    {
        public int CustomerId { get; set; } = 0;
        public string Token { get; set; } = string.Empty; 
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public string? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
