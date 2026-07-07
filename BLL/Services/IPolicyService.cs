using BLL.DTOs;
using DAL.Models;

namespace BLL.Services
{
    public interface IPolicyService
    {
        List<PolicyListResponseDTO> GetAllPolicies();
        int CreatePolicy(PolicyRequestDTO dto, int customerId);
        PolicyResponseDTO? GetPolicy(int policyId);
        List<PolicyResponseDTO> GetPoliciesByCustomer(int customerId);
        PolicyUnderwriterDTO? GetPolicyForUnderwriter(int policyId);
        bool ReviewPolicy(int policyId, UnderwriterReviewDTO dto);
    }
}