namespace SecuTrack.Core.Entities
{
    public class AuditSession : BaseEntity
    {
        public int ProviderId { get; set; }
        public Provider Provider { get; set; } = null!;

        public string AuditorName { get; set; } = string.Empty;
        public DateTime AuditDate { get; set; }
        public string AuditVersion { get; set; } = "1.0";
        public string Notes { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;

        // Relaciones
        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
        public ComplianceScore? ComplianceScore { get; set; }
    }
}