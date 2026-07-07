using System.ComponentModel.DataAnnotations;

namespace Ipms.Frontend.Models
{
        public class CreateClaimModel
        {
            [Required]
            [Display(Name = "Policy ID")]
            public int PolicyId { get; set; }

            [Display(Name = "Claim Date")]
            [DataType(DataType.Date)]
            public DateTime? ClaimDate { get; set; } = DateTime.Now;

            [Required]
            [Range(0.01, double.MaxValue, ErrorMessage = "Claim amount must be greater than 0")]
            [Display(Name = "Claim Amount")]
            public decimal ClaimAmount { get; set; }

            [Required]
            [MaxLength(500)]
            [Display(Name = "Reason")]
            public string Reason { get; set; } = null!;
        }
}
