using TitanFitness.Application.Bookings.Dtos;
using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Bookings.Queries.GetBookingsBySession
{
    public class GetBookingsBySessionQueryHandler
    {
        private readonly IGenericRepository<Booking> _bookingRepository;

        public GetBookingsBySessionQueryHandler(
            IGenericRepository<Booking> bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<List<BookingDto>> Handle(
            GetBookingsBySessionQuery query)
        {
            var bookings = await _bookingRepository.FindAsync(
                x => x.SessionId == query.SessionId);

            return bookings
                .OrderBy(x => x.BookedOn)
                .Select(x => new BookingDto
                {
                    BookingId = x.BookingId,
                    SessionId = x.SessionId,
                    MemberId = x.MemberId,
                    BookedOn = x.BookedOn,
                    Status = x.Status,
                    WaitlistPosition = x.WaitlistPosition,
                    NotesForTrainer = x.NotesForTrainer
                })
                .ToList();
        }
    }
}
