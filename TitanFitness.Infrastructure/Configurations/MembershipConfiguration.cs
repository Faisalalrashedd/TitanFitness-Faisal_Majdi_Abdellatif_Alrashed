using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Infrastructure.Configurations
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(x => x.MembershipId);

            builder.Property(x => x.PurchaseDate)
                .IsRequired();

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.HasOne(x => x.Member)
                .WithMany(x => x.Memberships)
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Plan)
                .WithMany(x => x.Memberships)
                .HasForeignKey(x => x.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // these values stay fixed after purchase
            // that tells EF Core that AgreedTerms belongs to Membership rather than being its own separate entity
            builder.OwnsOne(x => x.AgreedTerms, terms =>
            {
                terms.Property(x => x.PricePaid)
                    .HasPrecision(18, 2);

                terms.Property(x => x.DurationInMonths);

                terms.Property(x => x.MaximumFreezeDays);

                terms.Property(x => x.MaximumNumberOfFreezes);

                terms.Property(x => x.GuestPassQuota);

                terms.Property(x => x.AccessScope);
            });
        }
    }
}