using DAL.Models;

namespace DAL.Repositories
{
    public interface IAuditLogRepository
    {
        void Log(string entityName, int entityId, string actionType, int? userId);
        List<AuditLog> GetAll();
        List<AuditLog> GetByEntity(string entityName, int entityId);
        List<AuditLog> GetByUser(int userId);
    }
}