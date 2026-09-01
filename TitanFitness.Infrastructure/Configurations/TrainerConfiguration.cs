using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Infrastructure.Configurations
{
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.HasKey(x => x.TrainerId);

            builder.Property(x => x.TrainerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .HasMaxLength(100);

            builder.Property(x => x.Phone)
                .HasMaxLength(20);

            builder.Property(x => x.IsActive)
                .IsRequired();
        }
    }
}