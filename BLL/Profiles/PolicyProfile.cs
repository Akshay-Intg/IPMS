using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Routing.Constraints;

namespace BLL.Profiles
{
    public class PolicyProfile : Profile
    {
        public PolicyProfile()
        {
            CreateMap<PolicyRequestDTO, Policy>()
                .ForMember(dest => dest.PolicyId, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.PolicyNumber, opt => opt.Ignore())
                .ForMember(dest => dest.PolicyStatus, opt => opt.Ignore())
                .ForMember(dest => dest.PolicyDate, opt => opt.Ignore())
                .ForMember(dest => dest.MaturityDate, opt => opt.Ignore())
                .ForMember(dest => dest.PremiumAmount, opt => opt.Ignore())
                .ForMember(dest => dest.SumAssured, opt => opt.Ignore())
                .ForMember(dest => dest.Claims, opt => opt.Ignore())
                .ForMember(dest => dest.Payments, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Scheme, opt => opt.Ignore());

            CreateMap<Policy, PolicyResponseDTO>()
                .ForMember(dest => dest.SchemeName,
               opt => opt.MapFrom(src => src.Scheme != null ? src.Scheme.SchemeName : null))
                .ForMember(dest => dest.PaymentStatus,
                opt => opt.MapFrom(src => src.Payments
                    .OrderByDescending(p => p.PaymentDate)
                    .Select(p => p.PaymentStatus)
                    .FirstOrDefault()));

            CreateMap<Policy, PolicyListResponseDTO>()
                .ForMember(dest => dest.SchemeName,
               opt => opt.MapFrom(src => src.Scheme != null ? src.Scheme.SchemeName : null))
                .ForMember(dest => dest.CustomerName,
               opt => opt.MapFrom(src => src.Customer != null
                   ? src.Customer.FirstName + " " + src.Customer.LastName
                   : null));
            CreateMap<Policy, PolicyUnderwriterDTO>()
    .ForMember(dest => dest.SchemeName, opt => opt.MapFrom(src => src.Scheme != null ? src.Scheme.SchemeName : null))
    .ForMember(dest => dest.Age, opt => opt.Ignore())
    .ForMember(dest => dest.BMI, opt => opt.Ignore())
    .ForMember(dest => dest.BMICategory, opt => opt.Ignore())
    .ForMember(dest => dest.AgeFactor, opt => opt.Ignore())
    .ForMember(dest => dest.BMIFactor, opt => opt.Ignore())
    .ForMember(dest => dest.SmokerFactor, opt => opt.Ignore())
    .ForMember(dest => dest.BasePremium, opt => opt.Ignore())
    .ForMember(dest => dest.SuggestedPremium, opt => opt.Ignore())
    .ForMember(dest => dest.UnderwriterRemarks, opt => opt.MapFrom(src => src.UnderwriterRemarks));
        }
    }
}