using MyBookBackend.Domain.DomainModels;

namespace MyBookBackend.Repository.IRepository
{
    public interface IAuditRepository
    {
        Task Create(AuditLog auditLog);
    }
}
