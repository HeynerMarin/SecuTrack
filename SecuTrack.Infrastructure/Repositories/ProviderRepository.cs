using Microsoft.EntityFrameworkCore;
using SecuTrack.Core.Entities;
using SecuTrack.Core.Interfaces;
using SecuTrack.Infrastructure.Data;

namespace SecuTrack.Infrastructure.Repositories
{
    public class ProviderRepository : Repository<Provider>, IProviderRepository
    {
        public ProviderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Provider>> GetProvidersWithAuditsAsync()
        {
            return await _context.Providers
                .Include(p => p.AuditSessions)
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Provider?> GetProviderWithAuditsAsync(int id)
        {
            return await _context.Providers
                .Include(p => p.AuditSessions)
                    .ThenInclude(a => a.ComplianceScore)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }
    }
}