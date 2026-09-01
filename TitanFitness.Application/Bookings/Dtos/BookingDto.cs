using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.Bookings.Dtos
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public int SessionId { get; set; }
        public int MemberId { get; set; }
        public DateTime BookedOn { get; set; }
        public BookingStatus Status { get; set; }
        public int? WaitlistPosition { get; set; }
        public string? NotesForTrainer { get; set; }
    }
}
