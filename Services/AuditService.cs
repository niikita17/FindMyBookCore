using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Repository.IRepository;
using MyBook_Backend.Services.IServices;

namespace MyBook_Backend.Services
{
    public class AuditService : IAuditService
    {
        public readonly IAuditRepository _auditRepository;
        public AuditService(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }
        public async  Task Log(int userId, string action, string entityName, int entityId, string description)
        {
            AuditLog log =
            new AuditLog
            {
                UserId = userId,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                Description = description
            };

            await _auditRepository
                .Create(log);
        }
    }
}
