using AutoMapper;
using BLL.DTOs;
using DAL.Repositories;
namespace BLL.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repo;
        private readonly IMapper _mapper;

        public AuditLogService(IAuditLogRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public void Log(string entityName, int entityId, string actionType, int? userId)
        {
            _repo.Log(entityName, entityId, actionType, userId);
        }
            

        public List<AuditLogDTO> GetAll()
        {
            return _mapper.Map<List<AuditLogDTO>>(_repo.GetAll());
        }
             

        public List<AuditLogDTO> GetByEntity(string entityName, int entityId)
        {
            return _mapper.Map<List<AuditLogDTO>>(_repo.GetByEntity(entityName, entityId));
        }
            

        public List<AuditLogDTO> GetByUser(int userId)
        {
            return _mapper.Map<List<AuditLogDTO>>(_repo.GetByUser(userId));
        }
             
    }
}