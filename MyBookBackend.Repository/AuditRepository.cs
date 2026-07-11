using MyBookBackend.Domain.Data;
using MyBookBackend.Domain.DomainModels;
using MyBookBackend.Repository.IRepository;

namespace MyBookBackend.Repository
{
    public class AuditRepository : IAuditRepository
    {

        public readonly ApplicationDbContext _context;
       
        public AuditRepository(ApplicationDbContext context)
        {
            _context = context;
          
        }
        public async Task Create(AuditLog auditLog)
        {
           await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();
        }

    }
}
