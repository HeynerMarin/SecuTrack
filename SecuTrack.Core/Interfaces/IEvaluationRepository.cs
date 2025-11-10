using SecuTrack.Core.Entities;

namespace SecuTrack.Core.Interfaces
{
    public interface IEvaluationRepository : IRepository<Evaluation>
    {
        Task<IEnumerable<Evaluation>> GetEvaluationsByAuditAsync(int auditId);
        Task<Evaluation?> GetEvaluationWithControlAsync(int evaluationId);
    }
}