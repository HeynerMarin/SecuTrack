using Microsoft.EntityFrameworkCore;
using SecuTrack.Core.Entities;
using SecuTrack.Core.Interfaces;
using SecuTrack.Infrastructure.Data;

namespace SecuTrack.Infrastructure.Repositories
{
    public class AuditRepository : Repository<AuditSession>, IAuditRepository
    {
        public AuditRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AuditSession>> GetAuditsByProviderAsync(int providerId)
        {
            return await _context.AuditSessions
                .Include(a => a.Provider)
                .Include(a => a.ComplianceScore)
                .Where(a => a.ProviderId == providerId && a.IsActive)
                .OrderByDescending(a => a.AuditDate)
                .ToListAsync();
        }

        public async Task<AuditSession?> GetAuditWithDetailsAsync(int auditId)
        {
            return await _context.AuditSessions
                .Include(a => a.Provider)
                .Include(a => a.Evaluations)
                    .ThenInclude(e => e.ISOControl)
                .Include(a => a.ComplianceScore)
                .FirstOrDefaultAsync(a => a.Id == auditId && a.IsActive);
        }

        public async Task<IEnumerable<AuditSession>> GetCompletedAuditsAsync()
        {
            return await _context.AuditSessions
                .Include(a => a.Provider)
                .Include(a => a.ComplianceScore)
                .Where(a => a.IsCompleted && a.IsActive)
                .OrderByDescending(a => a.AuditDate)
                .ToListAsync();
        }
    }
}