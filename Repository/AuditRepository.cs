using MyBook_Backend.Data;
using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Repository.IRepository;

namespace MyBook_Backend.Repository
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
