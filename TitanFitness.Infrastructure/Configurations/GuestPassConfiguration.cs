using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Infrastructure.Configurations
{
    public class GuestPassConfiguration : IEntityTypeConfiguration<GuestPass>
    {
        public void Configure(EntityTypeBuilder<GuestPass> builder)
        {
            builder.HasKey(x => x.GuestPassId);

            builder.Property(x => x.IssuedOn)
                .IsRequired();

            builder.Property(x => x.GuestName)
                .HasMaxLength(100);

            // each guest pass belongs to one membership
            builder.HasOne(x => x.Membership)
                .WithMany(x => x.GuestPasses)
                .HasForeignKey(x => x.MembershipId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}