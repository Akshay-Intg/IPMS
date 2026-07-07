using DAL.Models;

namespace DAL.Repositories
{
    public interface IPolicyRepository
    {
        int SavePolicy(Policy policy);
        bool MarkAsPaid(int policyId, string policyNumber);
        Policy? GetPolicyById(int policyId);
        void UpdatePolicyNumber(int policyId, string policyNumber);
        List<Policy> GetPoliciesByCustomerId(int customerId);

        List<Policy> GetAllPolicies();
        bool UpdatePolicyByUnderwriter(int policyId, decimal premiumAmount, string status, string? remarks);
    }
}