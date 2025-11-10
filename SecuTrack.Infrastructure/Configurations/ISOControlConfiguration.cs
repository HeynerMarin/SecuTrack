using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecuTrack.Core.Entities;

namespace SecuTrack.Infrastructure.Configurations
{
    public class ISOControlConfiguration : IEntityTypeConfiguration<ISOControl>
    {
        public void Configure(EntityTypeBuilder<ISOControl> builder)
        {
            builder.ToTable("ISOControls");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ControlId)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(i => i.ControlId)
                .IsUnique();

            builder.Property(i => i.Title)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(i => i.Description)
                .HasMaxLength(2000);

            builder.Property(i => i.Purpose)
                .HasMaxLength(1000);

            builder.Property(i => i.CloudAdaptation)
                .HasMaxLength(2000);

            builder.HasMany(i => i.Evaluations)
                .WithOne(e => e.ISOControl)
                .HasForeignKey(e => e.ISOControlId);
        }
    }
}