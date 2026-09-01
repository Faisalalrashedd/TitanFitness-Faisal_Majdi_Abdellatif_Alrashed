namespace TitanFitness.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommand
    {
        public int SessionId { get; set; }
        public int MemberId { get; set; }
        public string? NotesForTrainer { get; set; }
    }
}
