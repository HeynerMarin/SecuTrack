using SecuTrack.Core.Enums;

namespace SecuTrack.Application.DTOs
{
    public class EvaluationDto
    {
        public int Id { get; set; }
        public int AuditSessionId { get; set; }
        public int ISOControlId { get; set; }
        public string ControlId { get; set; } = string.Empty;
        public string ControlTitle { get; set; } = string.Empty;
        public ISOCategory Category { get; set; }
        public ComplianceLevel ComplianceLevel { get; set; }
        public string ComplianceLevelDisplay { get; set; } = string.Empty;
        public string Evidence { get; set; } = string.Empty;
        public string AuditorNotes { get; set; } = string.Empty;
        public string Recommendations { get; set; } = string.Empty;
        public bool IsCritical { get; set; }
    }

    public class CreateEvaluationDto
    {
        public int AuditSessionId { get; set; }
        public int ISOControlId { get; set; }
        public ComplianceLevel ComplianceLevel { get; set; }
        public string Evidence { get; set; } = string.Empty;
        public string AuditorNotes { get; set; } = string.Empty;
        public string Recommendations { get; set; } = string.Empty;
    }
}