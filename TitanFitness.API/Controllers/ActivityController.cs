using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.Bookings.Queries.GetBookingsByMember;
using TitanFitness.Application.Bookings.Queries.GetBookingsBySession;
using TitanFitness.Application.CheckIns.Queries.GetCheckInsByMember;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class ActivityController : ControllerBase
    {
        private readonly GetBookingsBySessionQueryHandler _sessionBookingsHandler;
        private readonly GetBookingsByMemberQueryHandler _memberBookingsHandler;
        private readonly GetCheckInsByMemberQueryHandler _memberCheckInsHandler;

        public ActivityController(
            GetBookingsBySessionQueryHandler sessionBookingsHandler,
            GetBookingsByMemberQueryHandler memberBookingsHandler,
            GetCheckInsByMemberQueryHandler memberCheckInsHandler)
        {
            _sessionBookingsHandler = sessionBookingsHandler;
            _memberBookingsHandler = memberBookingsHandler;
            _memberCheckInsHandler = memberCheckInsHandler;
        }

        [HttpGet("sessions/{sessionId}/bookings")]
        public async Task<IActionResult> GetSessionBookings(int sessionId)
        {
            var bookings =
                await _sessionBookingsHandler.Handle(
                    new GetBookingsBySessionQuery
                    {
                        SessionId = sessionId
                    });

            return Ok(bookings);
        }

        [HttpGet("members/{memberId}/bookings")]
        public async Task<IActionResult> GetMemberBookings(int memberId)
        {
            var bookings =
                await _memberBookingsHandler.Handle(
                    new GetBookingsByMemberQuery
                    {
                        MemberId = memberId
                    });

            return Ok(bookings);
        }

        [HttpGet("members/{memberId}/check-ins")]
        public async Task<IActionResult> GetMemberCheckIns(int memberId)
        {
            var checkIns =
                await _memberCheckInsHandler.Handle(
                    new GetCheckInsByMemberQuery
                    {
                        MemberId = memberId
                    });

            return Ok(checkIns);
        }
    }
}
