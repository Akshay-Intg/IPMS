using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using Ipms.Frontend.DTOs.Deserialize;
using Ipms.Frontend.Models;
namespace Ipms.Frontend.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginResponseDTO, CustomerDeserializeDTO>();
            CreateMap<PolicyFormModel, PolicyRequestDTO>();

            CreateMap<PolicyRequestDeserialize,
                      PolicyRequestDTO>();

            CreateMap<Customer, CustomerDTO>()
             .ForMember(dest => dest.IsDeleted,
               opt => opt.MapFrom(src => src.IsDeleted));
            // or if using ReverseMap() it maps automatically
        }
    }
}
