using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingResult
    {
        public bool Success { get; set; }
        public int? BookingId { get; set; }
        public BookingStatus? Status { get; set; }
        public int? WaitlistPosition { get; set; }
        public string? Error { get; set; }
    }
}
