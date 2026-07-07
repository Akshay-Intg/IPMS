using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ipms.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditService;

        public AuditLogController(IAuditLogService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            return Ok(_auditService.GetAll());
        }
             

        [HttpGet("user/{userId}")]
        public IActionResult GetByUser(int userId)
        {
            if (userId == 0)
            {
                return BadRequest();
            }
            return Ok(_auditService.GetByUser(userId));
        }
             

        [HttpGet("entity/{entityName}/{entityId}")]
        public IActionResult GetByEntity(string entityName, int entityId)
        {
            if (entityName == null)
            {
                return BadRequest();
            }
            return Ok(_auditService.GetByEntity(entityName, entityId));
        }
            
    }
}