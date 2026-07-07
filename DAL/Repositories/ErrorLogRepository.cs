using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ErrorLogRepository:IErrorLogRepository
    {
        private readonly _IpmsContext _context;

        public ErrorLogRepository(_IpmsContext context)
        {
            _context = context;
        }
        public void ExceptionLogAsync(ErrorLog errorLog)
        {
            _context.ErrorLogs.Add(errorLog);
            _context.SaveChanges();
        }

        public async Task<List<ErrorLog>> ViewErrorLogAsync()
        {
            return await _context.ErrorLogs.ToListAsync();
        }
    }
}
