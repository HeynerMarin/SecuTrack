using Microsoft.EntityFrameworkCore;
using SecuTrack.Core.Entities;
using SecuTrack.Core.Interfaces;
using SecuTrack.Infrastructure.Data;

namespace SecuTrack.Infrastructure.Repositories
{
    public class EvaluationRepository : Repository<Evaluation>, IEvaluationRepository
    {
        public EvaluationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Evaluation>> GetEvaluationsByAuditAsync(int auditId)
        {
            return await _context.Evaluations
                .Include(e => e.ISOControl)
                .Where(e => e.AuditSessionId == auditId && e.IsActive)
                .OrderBy(e => e.ISOControl.ControlId)
                .ToListAsync();
        }

        public async Task<Evaluation?> GetEvaluationWithControlAsync(int evaluationId)
        {
            return await _context.Evaluations
                .Include(e => e.ISOControl)
                .Include(e => e.AuditSession)
                .FirstOrDefaultAsync(e => e.Id == evaluationId && e.IsActive);
        }
    }
}