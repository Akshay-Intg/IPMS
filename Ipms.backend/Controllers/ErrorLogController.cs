using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ipms.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorLogController : ControllerBase
    {
        private readonly IErrorLogService errorLogService;
        public ErrorLogController(IErrorLogService errorLogService)
        {
            this.errorLogService = errorLogService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllErrorLogs()
        {
            try
            {
                var errorlist = await errorLogService.GetAllAsync();
                if (errorlist.Count == 0)
                {
                    return NotFound();
                }
                return Ok(errorlist);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex);
            }

        }
    }
}
