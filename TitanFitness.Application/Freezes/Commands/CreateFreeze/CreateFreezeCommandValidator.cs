using FluentValidation;

namespace TitanFitness.Application.Freezes.Commands.CreateFreeze
{
    public class CreateFreezeCommandValidator : AbstractValidator<CreateFreezeCommand>
    {
        public CreateFreezeCommandValidator()
        {
            RuleFor(x => x.MembershipId)
                .GreaterThan(0);

            RuleFor(x => x.StartDate)
                .NotEmpty();

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartDate);

            RuleFor(x => x.Reason)
                .IsInEnum();

            RuleFor(x => x.AdditionalNotes)
                .MaximumLength(200);
        }
    }
}
