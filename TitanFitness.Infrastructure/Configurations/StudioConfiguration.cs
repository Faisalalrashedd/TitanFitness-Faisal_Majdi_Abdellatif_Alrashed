using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Infrastructure.Configurations
{
    public class StudioConfiguration : IEntityTypeConfiguration<Studio>
    {
        public void Configure(EntityTypeBuilder<Studio> builder)
        {
            builder.HasKey(x => x.StudioId);

            builder.Property(x => x.StudioName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Capacity)
                .IsRequired();

            // each studio belongs to one branch
            builder.HasOne(x => x.Branch)
                .WithMany(x => x.Studios)
                .HasForeignKey(x => x.BranchId);
        }
    }
}