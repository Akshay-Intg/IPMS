using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;

namespace BLL.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _repo;
        private readonly ISchemeRepository _schemeRepo;
        private readonly IMapper _mapper;
        private readonly IAuditLogRepository _auditRepo;
        private readonly IPaymentRepository _paymentRepo;

        public PolicyService(IPolicyRepository policyRepository,
                             ISchemeRepository schemeRepository,
                             IMapper mapper,
                             IAuditLogRepository auditRepo,
                             IPaymentRepository paymentRepo)
        {
            _repo = policyRepository;
            _schemeRepo = schemeRepository;
            _mapper = mapper;
            _auditRepo = auditRepo;
            _paymentRepo = paymentRepo;
        }

        public int CreatePolicy(PolicyRequestDTO dto, int customerId)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("Full name is required.");

            if (string.IsNullOrWhiteSpace(dto.PANCard) || dto.PANCard.Length != 10)
                throw new ArgumentException("Valid 10-character PAN Card is required.");

            if (dto.DOB == default || dto.DOB > DateTime.Now.AddYears(-18))
                throw new ArgumentException("Proposer must be at least 18 years old.");

            if (string.IsNullOrWhiteSpace(dto.NomineeName))
                throw new ArgumentException("Nominee name is required.");

            if (dto.SchemeId == 0)
                throw new ArgumentException("Invalid scheme selected.");

            var scheme = _schemeRepo.GetInsuranceSchemeByIdAsync(dto.SchemeId).Result;
            if (scheme == null)
                throw new ArgumentException("Scheme not found.");

            var sumAssured = dto.SumAssured ?? scheme.MinAmount;
            var policyTerm = dto.PolicyTerm ?? 1;
            var premiumAmount = sumAssured * (scheme.ProfitRatio / 100) / policyTerm;
            var policyDate = DateTime.Now;
            var maturityDate = policyDate.AddYears(policyTerm);

            var policy = _mapper.Map<Policy>(dto);

            policy.CustomerId = customerId;
            policy.SumAssured = sumAssured;
            policy.PremiumAmount = premiumAmount;
            policy.PolicyStatus = "Pending";
            policy.PolicyDate = policyDate;
            policy.MaturityDate = maturityDate;
            policy.PolicyNumber = "PENDING"; 

            var policyId = _repo.SavePolicy(policy);
            _auditRepo.Log("Policy", policyId, "Created", customerId);

            _repo.UpdatePolicyNumber(policyId,
                $"IPMS-{policyDate:yyyyMMdd}-{policyId:D5}");

            return policyId;
        }

        public PolicyResponseDTO? GetPolicy(int policyId)
        {
            var policy = _repo.GetPolicyById(policyId);
            return policy == null ? null : _mapper.Map<PolicyResponseDTO>(policy);
        }

        public List<PolicyResponseDTO> GetPoliciesByCustomer(int customerId)
        {
            var policies = _repo.GetPoliciesByCustomerId(customerId);
            return _mapper.Map<List<PolicyResponseDTO>>(policies);
        }

        public List<PolicyListResponseDTO> GetAllPolicies()
        {
            var policies = _repo.GetAllPolicies();
            return _mapper.Map<List<PolicyListResponseDTO>>(policies);
        }


        public PolicyUnderwriterDTO? GetPolicyForUnderwriter(int policyId)
        {
            var policy = _repo.GetPolicyById(policyId);
            if (policy == null) return null;

            var age = DateTime.Now.Year - (policy.DOB?.Year ?? DateTime.Now.Year);

            var totalInches = (policy.HeightFeet * 12) + policy.HeightInches;
            var heightMeters = totalInches * 0.0254m;
            var bmi = heightMeters > 0
                               ? Math.Round(policy.Weight / (heightMeters * heightMeters), 1)
                               : 0;

            var bmiCategory = bmi switch
            {
                < 18.5m => "Underweight",
                < 25.0m => "Normal",
                < 30.0m => "Overweight",
                _ => "Obese"
            };

            decimal ageFactor = age switch
            {
                <= 25 => 0.90m,
                <= 35 => 1.00m,
                <= 45 => 1.15m,
                <= 55 => 1.30m,
                _ => 1.50m
            };

            decimal bmiFactor = bmi switch
            {
                < 18.5m => 1.10m,
                < 25.0m => 1.00m,
                < 30.0m => 1.15m,
                _ => 1.30m
            };

            decimal smokerFactor = policy.Smokes ? 1.25m : 1.00m;

            var scheme = _schemeRepo.GetInsuranceSchemeByIdAsync(policy.SchemeId).Result;
            var policyTerm = policy.PolicyTerm ?? 1;
            var basePremium = scheme != null
                               ? policy.SumAssured * (scheme.ProfitRatio / 100) / policyTerm
                               : policy.PremiumAmount;
            var suggestedPremium = Math.Round(basePremium * ageFactor * bmiFactor * smokerFactor, 2);

            var dto = _mapper.Map<PolicyUnderwriterDTO>(policy);

            dto.Age = age;
            dto.BMI = bmi;
            dto.BMICategory = bmiCategory;
            dto.AgeFactor = ageFactor;
            dto.BMIFactor = bmiFactor;
            dto.SmokerFactor = smokerFactor;
            dto.BasePremium = Math.Round(basePremium, 2);
            dto.SuggestedPremium = suggestedPremium;
            dto.SchemeName = scheme?.SchemeName;

            return dto;
        }

        public bool ReviewPolicy(int policyId, UnderwriterReviewDTO dto)
        {
            if (dto.Status != "Active" && dto.Status != "Rejected")
                throw new ArgumentException("Status must be Active or Rejected.");

            var result = _repo.UpdatePolicyByUnderwriter(policyId, dto.PremiumAmount, dto.Status, dto.Remarks);
            if (result)
            {
                _auditRepo.Log("Policy", policyId, dto.Status == "Active" ? "Approved" : "Rejected", null);

                if (dto.Status == "Rejected")
                {
                    _paymentRepo.UpdatePaymentStatusByPolicyId(policyId, "Refunded");
                }

                return true;
            }
            return false;
        }
    }
}