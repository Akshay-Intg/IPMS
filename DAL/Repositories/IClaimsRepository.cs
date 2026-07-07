using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IClaimsRepository
    {
        Task<Claim> Apply(Claim claim);
        Task<bool> FindDuplicatePolicyId(int id);
        Task<List<Claim>> FindClaimsById(int customerId);
        Task<List<Claim>> AllClaimsAsync();
        Task<Claim> GetClaimByClaimIdAsync(int claimId);
        Task UpdateAsync(Claim claim);
        Task SaveChangesAsync();
    }
}
