using AutoMapper;
using BLL.DTOs;
using DAL.Data;
using DAL.Models;

namespace BLL.Profiles
{
    public class ErrorLogProfile: Profile
    {
        public ErrorLogProfile()
        {
            CreateMap<ErrorLogDTO, ErrorLog>();
            CreateMap<ErrorLog, ErrorLogDTO>();
        }
    }
}
