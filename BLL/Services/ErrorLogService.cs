using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogRepository errorLogRepository;
        public ErrorLogService(IMapper mapper,IErrorLogRepository errorLogRepository)
        {
            _mapper = mapper;
            this.errorLogRepository= errorLogRepository;
        }
        public async Task ExceptionLog(ErrorLogDTO request)
        {
            var errorLog=_mapper.Map<ErrorLog>(request);
            errorLogRepository.ExceptionLogAsync(errorLog);

        }

        public async Task<List<ErrorLogDTO>> GetAllAsync()
        {
            var errorLogList = await errorLogRepository.ViewErrorLogAsync();
            var errorLogs=_mapper.Map<List<ErrorLogDTO>>(errorLogList);
            return errorLogs;
        }
    }
}
