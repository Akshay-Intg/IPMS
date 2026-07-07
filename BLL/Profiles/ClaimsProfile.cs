using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Profiles
{
    public class ClaimsProfile:Profile
    {
        public ClaimsProfile()
        {
            CreateMap<CreateClaimDTO, Claim>()
                .ForMember(dest => dest.ClaimId, opt => opt.Ignore()) 
                .ForMember(dest => dest.ClaimStatus, opt => opt.MapFrom(_ => "Pending")) 
                .ForMember(dest => dest.Policy, opt => opt.Ignore());
            CreateMap<Claim, ClaimResponseDTO>();

            CreateMap<Claim, ClaimDTO>()
            .ForMember(dest => dest.PolicyNumber,
                       opt => opt.MapFrom(src => src.Policy.PolicyNumber));
        }
    }
}
