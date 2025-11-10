namespace SecuTrack.Application.DTOs
{
    public class ComplianceScoreDto
    {
        public int Id { get; set; }
        public int AuditSessionId { get; set; }
        public decimal GlobalScore { get; set; }
        public decimal OrganizationalScore { get; set; }
        public decimal PeopleScore { get; set; }
        public decimal PhysicalScore { get; set; }
        public decimal TechnologicalScore { get; set; }
        public int TotalControls { get; set; }
        public int EvaluatedControls { get; set; }
        public int CriticalGaps { get; set; }
        public string ComplianceLevel { get; set; } = string.Empty;
    }
}