using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BLL.Profiles
{
    public class InsuranceSchemeProfile : Profile
    {
        public InsuranceSchemeProfile()
        {
            CreateMap<InsuranceSchemeDTO, InsuranceScheme>()
                .ForMember(dest => dest.SchemeId, opt => opt.Ignore()) 
                .ForMember(dest => dest.Policies, opt => opt.Ignore()); 

            CreateMap<InsuranceScheme, InsuranceSchemeResponseDTO>()
                .ForMember(dest => dest.status, opt => opt.Ignore())
                .ForMember(dest => dest.Message, opt => opt.Ignore());

            CreateMap<InsuranceType, InsuranceTypeDTO>();
            CreateMap<InsuranceTypeDTO, InsuranceType>();
        }
    }
}