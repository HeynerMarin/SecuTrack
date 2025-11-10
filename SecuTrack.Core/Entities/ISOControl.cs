using SecuTrack.Core.Enums;

namespace SecuTrack.Core.Entities
{
    public class ISOControl : BaseEntity
    {
        public string ControlId { get; set; } = string.Empty; // Ej: "5.1", "8.2"
        public ISOCategory Category { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public int Weight { get; set; } = 1; // Peso para cálculo (1-5)
        public bool IsCritical { get; set; } = false;
        public string CloudAdaptation { get; set; } = string.Empty; // Adaptación específica para cloud

        // Relaciones
        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    }
}