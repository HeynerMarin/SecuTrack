using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecuTrack.Core.Entities;

namespace SecuTrack.Infrastructure.Configurations
{
    public class AuditSessionConfiguration : IEntityTypeConfiguration<AuditSession>
    {
        public void Configure(EntityTypeBuilder<AuditSession> builder)
        {
            builder.ToTable("AuditSessions");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.AuditorName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.AuditVersion)
                .HasMaxLength(20);

            builder.Property(a => a.Notes)
                .HasMaxLength(2000);

            builder.HasMany(a => a.Evaluations)
                .WithOne(e => e.AuditSession)
                .HasForeignKey(e => e.AuditSessionId);

            builder.HasOne(a => a.ComplianceScore)
                .WithOne(c => c.AuditSession)
                .HasForeignKey<ComplianceScore>(c => c.AuditSessionId);
        }
    }
}