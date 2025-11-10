namespace SecuTrack.Application.DTOs
{
    public class AuditDto
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; } = string.Empty;
        public string AuditorName { get; set; } = string.Empty;
        public DateTime AuditDate { get; set; }
        public string AuditVersion { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public decimal? GlobalScore { get; set; }
        public int TotalEvaluations { get; set; }
        public int CriticalGaps { get; set; }
    }

    public class CreateAuditDto
    {
        public int ProviderId { get; set; }
        public string AuditorName { get; set; } = string.Empty;
        public DateTime AuditDate { get; set; } = DateTime.Now;
        public string AuditVersion { get; set; } = "1.0";
        public string Notes { get; set; } = string.Empty;
    }

    public class AuditDetailDto
    {
        public int Id { get; set; }
        public string ProviderName { get; set; } = string.Empty;
        public string AuditorName { get; set; } = string.Empty;
        public DateTime AuditDate { get; set; }
        public bool IsCompleted { get; set; }
        public ComplianceScoreDto? ComplianceScore { get; set; }
        public List<EvaluationDto> Evaluations { get; set; } = new();
    }
}