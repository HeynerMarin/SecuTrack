using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecuTrack.Core.Entities;

namespace SecuTrack.Infrastructure.Configurations
{
    public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.ToTable("Providers");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Region)
                .HasMaxLength(100);

            builder.Property(p => p.ContactEmail)
                .HasMaxLength(200);

            builder.Property(p => p.WebsiteUrl)
                .HasMaxLength(500);

            builder.Property(p => p.Description)
                .HasMaxLength(1000);

            builder.HasMany(p => p.AuditSessions)
                .WithOne(a => a.Provider)
                .HasForeignKey(a => a.ProviderId);
        }
    }
}