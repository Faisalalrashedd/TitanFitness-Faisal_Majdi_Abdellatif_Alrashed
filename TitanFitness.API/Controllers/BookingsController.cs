using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.Bookings.Commands.CancelBooking;
using TitanFitness.Application.Bookings.Commands.CreateBooking;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class BookingsController : ControllerBase
    {
        private readonly CreateBookingCommandHandler _createHandler;
        private readonly CancelBookingCommandHandler _cancelHandler;
        private readonly IValidator<CreateBookingCommand> _validator;

        public BookingsController(
            CreateBookingCommandHandler createHandler,
            CancelBookingCommandHandler cancelHandler,
            IValidator<CreateBookingCommand> validator)
        {
            _createHandler = createHandler;
            _cancelHandler = cancelHandler;
            _validator = validator;
        }

        [HttpPost("sessions/{sessionId}/bookings")]
        public async Task<IActionResult> CreateBooking(
            int sessionId,
            CreateBookingCommand command)
        {
            command.SessionId = sessionId;

            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _createHandler.Handle(command);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    Message = result.Error
                });
            }

            return Ok(new
            {
                BookingId = result.BookingId,
                Status = result.Status,
                WaitlistPosition = result.WaitlistPosition
            });
        }

        [HttpDelete("bookings/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var cancelled =
                await _cancelHandler.Handle(new CancelBookingCommand
                {
                    BookingId = id
                });

            if (!cancelled)
            {
                return NotFound();
            }

            return Ok(new
            {
                Message = "Booking cancelled successfully"
            });
        }
    }
}
