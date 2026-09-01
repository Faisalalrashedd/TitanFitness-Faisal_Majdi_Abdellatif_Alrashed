using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Infrastructure.Configurations
{
    // configures the class session table and its relationships
    public class ClassSessionConfiguration : IEntityTypeConfiguration<ClassSession>
    {
        public void Configure(EntityTypeBuilder<ClassSession> builder)
        {
            builder.HasKey(x => x.SessionId);

            builder.Property(x => x.ClassName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.SessionDate)
                .IsRequired();

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.DurationInMinutes)
                .IsRequired();

            builder.Property(x => x.CapacityLimit)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Studio)
                .WithMany()
                .HasForeignKey(x => x.StudioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Trainer)
                .WithMany(x => x.ClassSessions)
                .HasForeignKey(x => x.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}