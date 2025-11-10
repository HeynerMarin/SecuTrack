using SecuTrack.Core.Entities;

namespace SecuTrack.Core.Interfaces
{
    public interface IAuditRepository : IRepository<AuditSession>
    {
        Task<IEnumerable<AuditSession>> GetAuditsByProviderAsync(int providerId);
        Task<AuditSession?> GetAuditWithDetailsAsync(int auditId);
        Task<IEnumerable<AuditSession>> GetCompletedAuditsAsync();
    }
}