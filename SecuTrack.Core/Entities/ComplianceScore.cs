namespace SecuTrack.Core.Entities
{
    public class ComplianceScore : BaseEntity
    {
        public int AuditSessionId { get; set; }
        public AuditSession AuditSession { get; set; } = null!;

        public decimal GlobalScore { get; set; }
        public decimal OrganizationalScore { get; set; }
        public decimal PeopleScore { get; set; }
        public decimal PhysicalScore { get; set; }
        public decimal TechnologicalScore { get; set; }

        public int TotalControls { get; set; }
        public int EvaluatedControls { get; set; }
        public int CriticalGaps { get; set; }
    }
}