using SecuTrack.Core.Enums;

namespace SecuTrack.Core.Entities
{
    public class Provider : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ProviderType Type { get; set; }
        public string Region { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;

        // Relaciones
        public ICollection<AuditSession> AuditSessions { get; set; } = new List<AuditSession>();
    }
}
