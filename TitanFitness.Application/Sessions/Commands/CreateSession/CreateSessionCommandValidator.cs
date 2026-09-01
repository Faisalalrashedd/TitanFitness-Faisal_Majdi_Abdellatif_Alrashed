using FluentValidation;

namespace TitanFitness.Application.Sessions.Commands.CreateSession
{
    public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
    {
        public CreateSessionCommandValidator()
        {
            RuleFor(x => x.ClassName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.BranchId)
                .GreaterThan(0);

            RuleFor(x => x.StudioId)
                .GreaterThan(0);

            RuleFor(x => x.TrainerId)
                .GreaterThan(0);

            RuleFor(x => x.SessionDate)
                .NotEmpty();

            RuleFor(x => x.StartTime)
                .NotEmpty();

            RuleFor(x => x.DurationInMinutes)
                .GreaterThan(0);

            RuleFor(x => x.CapacityLimit)
                .GreaterThan(0);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }
}
