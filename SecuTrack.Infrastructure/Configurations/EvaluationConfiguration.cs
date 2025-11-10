using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecuTrack.Core.Entities;

namespace SecuTrack.Infrastructure.Configurations
{
    public class EvaluationConfiguration : IEntityTypeConfiguration<Evaluation>
    {
        public void Configure(EntityTypeBuilder<Evaluation> builder)
        {
            builder.ToTable("Evaluations");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Evidence)
                .HasMaxLength(2000);

            builder.Property(e => e.AuditorNotes)
                .HasMaxLength(2000);

            builder.Property(e => e.Recommendations)
                .HasMaxLength(2000);

            // Índice compuesto para evitar duplicados
            builder.HasIndex(e => new { e.AuditSessionId, e.ISOControlId })
                .IsUnique();
        }
    }
}