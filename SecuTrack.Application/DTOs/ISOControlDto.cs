using SecuTrack.Core.Enums;

namespace SecuTrack.Application.DTOs
{
    public class ISOControlDto
    {
        public int Id { get; set; }
        public string ControlId { get; set; } = string.Empty;
        public ISOCategory Category { get; set; }
        public string CategoryDisplay { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public int Weight { get; set; }
        public bool IsCritical { get; set; }
        public string CloudAdaptation { get; set; } = string.Empty;
    }
}