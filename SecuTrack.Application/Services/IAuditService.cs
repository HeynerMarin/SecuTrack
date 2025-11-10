using SecuTrack.Application.DTOs;

namespace SecuTrack.Application.Services
{
    public interface IAuditService
    {
        Task<IEnumerable<AuditDto>> GetAllAuditsAsync();
        Task<IEnumerable<AuditDto>> GetAuditsByProviderAsync(int providerId);
        Task<AuditDetailDto?> GetAuditDetailAsync(int auditId);
        Task<AuditDto> CreateAuditAsync(CreateAuditDto dto);
        Task<bool> CompleteAuditAsync(int auditId);
        Task<ComplianceScoreDto?> CalculateComplianceScoreAsync(int auditId);
    }
}