using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;
using TitanFitness.Domain.Enums;
using MemberEntity = TitanFitness.Domain.Entities.Member;

namespace TitanFitness.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler
    {
        private readonly IGenericRepository<ClassSession> _sessionRepository;
        private readonly IGenericRepository<MemberEntity> _memberRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IGenericRepository<Freeze> _freezeRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookingCommandHandler(
            IGenericRepository<ClassSession> sessionRepository,
            IGenericRepository<MemberEntity> memberRepository,
            IGenericRepository<Membership> membershipRepository,
            IGenericRepository<Freeze> freezeRepository,
            IGenericRepository<Booking> bookingRepository,
            IUnitOfWork unitOfWork)
        {
            _sessionRepository = sessionRepository;
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
            _freezeRepository = freezeRepository;
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateBookingResult> Handle(CreateBookingCommand command)
        {
            var session = await _sessionRepository.GetByIdAsync(command.SessionId);
            var member = await _memberRepository.GetByIdAsync(command.MemberId);

            if (session == null || member == null)
            {
                return new CreateBookingResult
                {
                    Error = "Session or member not found"
                };
            }

            var sessionStart =
                session.SessionDate.ToDateTime(session.StartTime);

            if (DateTime.Now >= sessionStart)
            {
                return new CreateBookingResult
                {
                    Error = "Bookings are closed for this session"
                };
            }

            var duplicate = await _bookingRepository.AnyAsync(x =>
                x.SessionId == command.SessionId &&
                x.MemberId == command.MemberId &&
                x.Status != BookingStatus.Cancelled);

            if (duplicate)
            {
                return new CreateBookingResult
                {
                    Error = "Member already has a booking for this session"
                };
            }

            var activeMemberships = await _membershipRepository.FindAsync(x =>
                x.MemberId == command.MemberId &&
                x.StartDate <= session.SessionDate &&
                x.EndDate >= session.SessionDate &&
                x.Status != MembershipStatus.Cancelled &&
                x.Status != MembershipStatus.Expired);

            var membership = activeMemberships.FirstOrDefault();

            if (membership == null)
            {
                return new CreateBookingResult
                {
                    Error = "Member does not have an active membership for this session date"
                };
            }

            var freezes = await _freezeRepository.FindAsync(x =>
                x.MembershipId == membership.MembershipId &&
                x.StartDate <= session.SessionDate &&
                x.EndDate >= session.SessionDate);

            if (freezes.Any())
            {
                return new CreateBookingResult
                {
                    Error = "Membership is frozen on this date"
                };
            }

            var memberBookings = await _bookingRepository.FindAsync(x =>
                x.MemberId == command.MemberId &&
                x.Status != BookingStatus.Cancelled);

            var allSessions = await _sessionRepository.GetAllAsync();

            var overlapping = memberBookings
                .Join(
                    allSessions,
                    booking => booking.SessionId,
                    item => item.SessionId,
                    (booking, item) => item)
                .Any(item =>
                {
                    if (item.SessionDate != session.SessionDate)
                    {
                        return false;
                    }

                    var existingStart = item.StartTime;
                    var existingEnd =
                        item.StartTime.AddMinutes(item.DurationInMinutes);

                    var newStart = session.StartTime;
                    var newEnd =
                        session.StartTime.AddMinutes(session.DurationInMinutes);

                    return existingStart < newEnd && existingEnd > newStart;
                });

            if (overlapping)
            {
                return new CreateBookingResult
                {
                    Error = "Member already has an overlapping session"
                };
            }

            var sessionBookings = await _bookingRepository.FindAsync(x =>
                x.SessionId == command.SessionId &&
                x.Status != BookingStatus.Cancelled);

            var bookedCount =
                sessionBookings.Count(x => x.Status == BookingStatus.Booked);

            BookingStatus status;
            int? waitlistPosition = null;

            if (bookedCount < session.CapacityLimit)
            {
                status = BookingStatus.Booked;
            }
            else
            {
                status = BookingStatus.Waitlisted;

                waitlistPosition =
                    sessionBookings
                        .Where(x => x.Status == BookingStatus.Waitlisted)
                        .Select(x => x.WaitlistPosition ?? 0)
                        .DefaultIfEmpty(0)
                        .Max() + 1;
            }

            var booking = new Booking
            {
                SessionId = command.SessionId,
                MemberId = command.MemberId,
                BookedOn = DateTime.Now,
                Status = status,
                WaitlistPosition = waitlistPosition,
                NotesForTrainer = command.NotesForTrainer
            };

            await _bookingRepository.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return new CreateBookingResult
            {
                Success = true,
                BookingId = booking.BookingId,
                Status = booking.Status,
                WaitlistPosition = booking.WaitlistPosition
            };
        }
    }
}
