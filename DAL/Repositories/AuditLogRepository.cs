using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly _IpmsContext _context;

        public AuditLogRepository(_IpmsContext context)
        {
            _context = context;
        }

        public void Log(string entityName, int entityId, string actionType, int? userId)
        {
            var log = new AuditLog
            {
                EntityName = entityName,
                EntityId = entityId,
                ActionType = actionType,
                ActionByUserId = userId,
                ActionDate = DateTime.UtcNow
            };
            _context.AuditLogs.Add(log);
            _context.SaveChanges();
            Serilog.Log.Information(
            "AuditLog => User:{UserId} Action:{Action} Entity:{Entity} EntityId:{EntityId}",
            userId,
            actionType,
            entityName,
            entityId
            );
        }

        public List<AuditLog> GetAll()
        {
            return _context.AuditLogs
                .OrderByDescending(a => a.ActionDate)
                .ToList();
        }

        public List<AuditLog> GetByEntity(string entityName, int entityId)
        {
            return _context.AuditLogs
                .Where(a => a.EntityName == entityName && a.EntityId == entityId)
                .OrderByDescending(a => a.ActionDate)
                .ToList();
        }

        public List<AuditLog> GetByUser(int userId)
        {
            return _context.AuditLogs
                .Where(a => a.ActionByUserId == userId)
                .OrderByDescending(a => a.ActionDate)
                .ToList();
        }
    }
}