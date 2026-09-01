using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Infrastructure.Configurations
{
    public class CheckInConfiguration : IEntityTypeConfiguration<CheckIn>
    {
        public void Configure(EntityTypeBuilder<CheckIn> builder)
        {
            builder.HasKey(x => x.CheckInId);

            builder.Property(x => x.CheckInDateTime)
                .IsRequired();

            builder.Property(x => x.Result)
                .IsRequired();

            builder.Property(x => x.RefusalReason)
                .HasMaxLength(100);

            builder.HasOne(x => x.Member)
                .WithMany(x => x.CheckIns)
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}