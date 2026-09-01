using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Infrastructure.Configurations
{
    // configures the booking table and its relationships
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.BookingId);

            builder.Property(x => x.BookedOn)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.NotesForTrainer)
                .HasMaxLength(500);

            builder.HasOne(x => x.ClassSession)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Member)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}