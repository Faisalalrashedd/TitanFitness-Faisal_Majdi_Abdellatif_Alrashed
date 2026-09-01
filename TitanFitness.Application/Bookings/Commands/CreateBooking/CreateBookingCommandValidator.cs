using FluentValidation;

namespace TitanFitness.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.SessionId)
                .GreaterThan(0);

            RuleFor(x => x.MemberId)
                .GreaterThan(0);

            RuleFor(x => x.NotesForTrainer)
                .MaximumLength(500);
        }
    }
}
