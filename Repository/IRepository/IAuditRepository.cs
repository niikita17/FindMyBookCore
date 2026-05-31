using MyBook_Backend.Models.DomainModels;

namespace MyBook_Backend.Repository.IRepository
{
    public interface IAuditRepository
    {
        Task Create(AuditLog auditLog);
    }
}
