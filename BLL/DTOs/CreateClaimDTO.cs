using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class CreateClaimDTO
    {
        public int PolicyId { get; set; }

        public DateTime? ClaimDate { get; set; }

        public decimal ClaimAmount { get; set; }

        public string Reason { get; set; } = null!;
    }
}
