using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Infrastructure.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasKey(x => x.BranchId);

            builder.Property(x => x.BranchName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Address)
                .HasMaxLength(200);

            builder.HasMany(x => x.Studios)
                .WithOne(x => x.Branch)
                .HasForeignKey(x => x.BranchId);
        }
    }
}