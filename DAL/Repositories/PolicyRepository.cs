using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly _IpmsContext _context;

        public PolicyRepository(_IpmsContext context)
        {
            _context = context;
        }

        public int SavePolicy(Policy policy)
        {
            _context.Policies.Add(policy);
            _context.SaveChanges();
            return policy.PolicyId; // EF auto-fills this after SaveChanges
        }

        public bool MarkAsPaid(int policyId, string policyNumber)
        {
            var policy = _context.Policies.Find(policyId);
            if (policy == null) return false;

            policy.PolicyStatus = "Active";
            policy.PolicyNumber = policyNumber;
            _context.SaveChanges();
            return true;
        }

        public Policy? GetPolicyById(int policyId)
        {
            return _context.Policies.
                Include(p => p.Scheme).
                Include(p=>p.Payments).
                FirstOrDefault(p => p.PolicyId == policyId);
        }

        public List<Policy> GetPoliciesByCustomerId(int customerId)
        {
            return _context.Policies
                .Where(p => p.CustomerId == customerId)
                .Include(p => p.Payments)
                .OrderByDescending(p => p.PolicyDate)
                .ToList();
        }
        public void UpdatePolicyNumber(int policyId, string policyNumber)
        {
            var policy = _context.Policies.Find(policyId);
            if (policy == null) return;
            policy.PolicyNumber = policyNumber;
            _context.SaveChanges();
        }
        public List<Policy> GetAllPolicies()
        {
            return _context.Policies
                .Include(p => p.Scheme)
                .Include(p => p.Customer)
                .OrderByDescending(p => p.PolicyDate)
                .ToList();
        }
        public bool UpdatePolicyByUnderwriter(int policyId, decimal premiumAmount, string status, string? remarks)
        {
            var policy = _context.Policies.Find(policyId);
            if (policy == null) return false;

            policy.PremiumAmount = premiumAmount;
            policy.PolicyStatus = status;
            policy.UnderwriterRemarks = remarks;
            _context.SaveChanges();
            return true;
        }
    }
}
