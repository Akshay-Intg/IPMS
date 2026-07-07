using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IClaimsService
    {
        Task<ClaimResponseDTO> ApplyClaimAsync(CreateClaimDTO request);
        Task<List<ClaimDTO>> GetClaimsAsync(int customerId); 
        Task<List<ClaimDTO>> GetAllClaimsAsync();
        Task<ClaimDTO> GetClaimByIdAsync(int id);
        Task<bool> UpdateClaimStatusAsync(int claimId, string newStatus);
    }
}
