using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Infrastructure.Configurations
{
    public class FreezeConfiguration : IEntityTypeConfiguration<Freeze>
    {
        public void Configure(EntityTypeBuilder<Freeze> builder)
        {
            builder.HasKey(x => x.FreezeId);

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired();

            builder.Property(x => x.DurationInMonths)
                .IsRequired();

            builder.Property(x => x.Reason)
                .IsRequired();

            builder.Property(x => x.AdditionalNotes)
                .HasMaxLength(200);

            builder.Property(x => x.RequestedOn)
                .IsRequired();

            // each freeze belongs to one membership
            builder.HasOne(x => x.Membership)
                .WithMany(x => x.Freezes)
                .HasForeignKey(x => x.MembershipId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}