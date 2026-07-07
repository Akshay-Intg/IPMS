using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IErrorLogService
    {
        Task ExceptionLog(ErrorLogDTO request);
        Task<List<ErrorLogDTO>> GetAllAsync();
    }
}
