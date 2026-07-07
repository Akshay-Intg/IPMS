using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ClaimsRepository : IClaimsRepository
    {
        private readonly _IpmsContext _context;
        public ClaimsRepository(_IpmsContext context)
        {
            _context = context;
        }

        public async Task<List<Claim>> AllClaimsAsync()
        {
            return await _context.Claims.Include(c=>c.Policy).ToListAsync();
        }

        public async Task<Claim> Apply(Claim claim)
        {
            await _context.AddAsync(claim);
            await _context.SaveChangesAsync();
            return claim;
        }

        public async Task UpdateAsync(Claim claim)
        {
            _context.Claims.Update(claim);
            await Task.CompletedTask;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<Claim>> FindClaimsById(int customerId)
        {
            return await _context.Claims
        .Include(c => c.Policy)
        .Where(c => c.Policy.CustomerId == customerId)
        .ToListAsync();
        }

        public async Task<bool> FindDuplicatePolicyId(int id)
        {
            return await _context.Claims.AnyAsync(c=>c.PolicyId == id);
        }

        public async Task<Claim> GetClaimByClaimIdAsync(int claimId)
        {
            return await _context.Claims.Include(c=>c.Policy).FirstOrDefaultAsync(c=>c.ClaimId==claimId);
        }
    }
}
