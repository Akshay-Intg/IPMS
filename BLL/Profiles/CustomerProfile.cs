using AutoMapper;
using BLL.DTOs;
using DAL.Data;
using DAL.Models;

namespace BLL.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<RegisterDto, Customer>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 3))
                .ForMember(dest => dest.DateOfBirth,
    opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Customer, ResponseDto>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));

            CreateMap<Customer, LoginResponseDTO>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .ForMember(dest => dest.Token, opt => opt.Ignore());

            CreateMap<Customer, CustomerProfileDto>()
    .ForMember(dest => dest.DateOfBirth,
               opt => opt.MapFrom(src => src.DateOfBirth.HasValue
                   ? DateOnly.FromDateTime(src.DateOfBirth.Value)
                   : DateOnly.MinValue));
            CreateMap<Customer, CustomerDTO>()
    .ForMember(dest => dest.CustomerID,
               opt => opt.MapFrom(src => src.CustomerId))
    .ForMember(dest => dest.DateOfBirth,
               opt => opt.MapFrom(src => src.DateOfBirth.HasValue
                   ? DateOnly.FromDateTime(src.DateOfBirth.Value)
                   : DateOnly.MinValue));

            CreateMap<CustomerProfileDto, Customer>()
    .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
    .ForMember(dest => dest.RoleId, opt => opt.Ignore())
    .ForMember(dest => dest.IsActive, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
    .ForMember(dest => dest.DateOfBirth,
               opt => opt.MapFrom(src => src.DateOfBirth.ToDateTime(TimeOnly.MinValue)));

            CreateMap<AdminUpdateCustomerDto, Customer>()
    .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
    .ForMember(dest => dest.DateOfBirth,
               opt => opt.MapFrom(src =>
                   src.DateOfBirth.ToDateTime(TimeOnly.MinValue)));  
        }
    }
}