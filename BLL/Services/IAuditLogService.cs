using BLL.DTOs;

namespace BLL.Services
{
    public interface IAuditLogService
    {
        void Log(string entityName, int entityId, string actionType, int? userId);
        List<AuditLogDTO> GetAll();
        List<AuditLogDTO> GetByEntity(string entityName, int entityId);
        List<AuditLogDTO> GetByUser(int userId);
    }
}