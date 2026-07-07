using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IErrorLogRepository
    {
        void ExceptionLogAsync(ErrorLog errorLog);
        Task<List<ErrorLog>> ViewErrorLogAsync();

    }
}
