using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class ErrorLogDTO
    {
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public DateTime LoggedDate { get; set; }

        public string RequestPath { get; set; }
        public string HttpMethod { get; set; }
        public string UserName { get; set; }
        public int StatusCode { get; set; }
    }
}
