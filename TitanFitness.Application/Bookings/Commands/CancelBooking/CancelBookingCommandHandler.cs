using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;
using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.Bookings.Commands.CancelBooking
{
    public class CancelBookingCommandHandler
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelBookingCommandHandler(
            IGenericRepository<Booking> bookingRepository,
            IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CancelBookingCommand command)
        {
            var booking =
                await _bookingRepository.GetByIdAsync(command.BookingId);

            if (booking == null || booking.Status == BookingStatus.Cancelled)
            {
                return false;
            }

            var wasBooked = booking.Status == BookingStatus.Booked;

            booking.Status = BookingStatus.Cancelled;
            booking.WaitlistPosition = null;

            _bookingRepository.Update(booking);

            if (wasBooked)
            {
                var sessionBookings = await _bookingRepository.FindAsync(x =>
                    x.SessionId == booking.SessionId &&
                    x.Status == BookingStatus.Waitlisted);

                var firstWaitlisted = sessionBookings
                    .OrderBy(x => x.WaitlistPosition)
                    .ThenBy(x => x.BookedOn)
                    .FirstOrDefault();

                if (firstWaitlisted != null)
                {
                    firstWaitlisted.Status = BookingStatus.Booked;
                    firstWaitlisted.WaitlistPosition = null;

                    _bookingRepository.Update(firstWaitlisted);

                    var remaining = sessionBookings
                        .Where(x => x.BookingId != firstWaitlisted.BookingId)
                        .OrderBy(x => x.WaitlistPosition)
                        .ThenBy(x => x.BookedOn)
                        .ToList();

                    for (int i = 0; i < remaining.Count; i++)
                    {
                        remaining[i].WaitlistPosition = i + 1;
                        _bookingRepository.Update(remaining[i]);
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
