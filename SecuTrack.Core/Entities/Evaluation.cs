using SecuTrack.Core.Enums;

namespace SecuTrack.Core.Entities
{
    public class Evaluation : BaseEntity
    {
        public int AuditSessionId { get; set; }
        public AuditSession AuditSession { get; set; } = null!;

        public int ISOControlId { get; set; }
        public ISOControl ISOControl { get; set; } = null!;

        public ComplianceLevel ComplianceLevel { get; set; }
        public string Evidence { get; set; } = string.Empty; // URLs, documentos
        public string AuditorNotes { get; set; } = string.Empty;
        public string Recommendations { get; set; } = string.Empty;
    }
}