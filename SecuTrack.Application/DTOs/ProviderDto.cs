using SecuTrack.Core.Enums;

namespace SecuTrack.Application.DTOs
{
    public class ProviderDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ProviderType Type { get; set; }
        public string TypeDisplay { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public int TotalAudits { get; set; }
        public DateTime? LastAuditDate { get; set; }
        public decimal? LastComplianceScore { get; set; }
    }

    public class CreateProviderDto
    {
        public string Name { get; set; } = string.Empty;
        public ProviderType Type { get; set; }
        public string Region { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
    }
}