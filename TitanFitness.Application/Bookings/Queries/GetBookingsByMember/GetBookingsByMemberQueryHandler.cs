using TitanFitness.Application.Bookings.Dtos;
using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Bookings.Queries.GetBookingsByMember
{
    public class GetBookingsByMemberQueryHandler
    {
        private readonly IGenericRepository<Booking> _bookingRepository;

        public GetBookingsByMemberQueryHandler(
            IGenericRepository<Booking> bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<List<BookingDto>> Handle(
            GetBookingsByMemberQuery query)
        {
            var bookings = await _bookingRepository.FindAsync(
                x => x.MemberId == query.MemberId);

            return bookings
                .OrderByDescending(x => x.BookedOn)
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
