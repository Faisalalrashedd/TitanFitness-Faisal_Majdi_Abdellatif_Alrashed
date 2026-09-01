using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Infrastructure.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.HasKey(x => x.PlanId);

            builder.Property(x => x.PlanName)
                .IsRequired()
                .HasMaxLength(50);

            // price only needs two decimal places
            builder.Property(x => x.Price)
                .HasPrecision(18, 2);

            builder.Property(x => x.DurationInMonths)
                .IsRequired();

            builder.Property(x => x.AccessScope)
                .IsRequired();

            builder.Property(x => x.IsPublished)
                .IsRequired();
        }
    }
}