using AutoMapper;
using BLL.DTOs;
using BLL.Profiles;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IMapper _mapper;
        private readonly IClaimsRepository claimsRepository;
        private readonly IPolicyRepository policyRepository;
        public ClaimsService(IMapper mapper,IClaimsRepository claimsRepository, IPolicyRepository policyRepository)
        {
            _mapper = mapper;
            this.claimsRepository = claimsRepository;
            this.policyRepository = policyRepository;
        }
        public async Task<ClaimResponseDTO> ApplyClaimAsync(CreateClaimDTO request)
        {
            var claim = _mapper.Map<Claim>(request);
            if (!await claimsRepository.FindDuplicatePolicyId(claim.PolicyId)) 
            {
                var policy=policyRepository.GetPolicyById(claim.PolicyId);
                if (policy.SumAssured < claim.ClaimAmount)
                {
                    return new ClaimResponseDTO
                    {
                        ClaimId = -1,
                    };
                }
                await claimsRepository.Apply(claim);
                var response = _mapper.Map<ClaimResponseDTO>(claim);
                return response;
            }
            return new ClaimResponseDTO
            {
                ClaimId=0,
            };
        }

        public async Task<List<ClaimDTO>> GetAllClaimsAsync()
        {
            var claimList=await claimsRepository.AllClaimsAsync();
            var list=_mapper.Map<List<ClaimDTO>>(claimList);
            return list;
        }

        public async Task<ClaimDTO> GetClaimByIdAsync(int id)
        {
            var claim=await claimsRepository.GetClaimByClaimIdAsync(id);
            var claimById=_mapper.Map<ClaimDTO>(claim);
            return claimById;
        }

        public async Task<List<ClaimDTO>> GetClaimsAsync(int customerId)
        {
            var claimList=await claimsRepository.FindClaimsById(customerId);
            var list=_mapper.Map<List<ClaimDTO>>(claimList);
            return list;    
        }
        public async Task<bool> UpdateClaimStatusAsync(int claimId, string newStatus)
        {
            var claim = await claimsRepository.GetClaimByClaimIdAsync(claimId);

            if (claim == null)
                return false;

            claim.ClaimStatus = newStatus;

            await claimsRepository.UpdateAsync(claim);
            await claimsRepository.SaveChangesAsync();

            return true;
        }
    }
}
