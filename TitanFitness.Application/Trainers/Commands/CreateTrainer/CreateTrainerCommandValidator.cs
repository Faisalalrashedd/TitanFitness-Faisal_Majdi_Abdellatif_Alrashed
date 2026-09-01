using FluentValidation;

namespace TitanFitness.Application.Trainers.Commands.CreateTrainer
{
    public class CreateTrainerCommandValidator : AbstractValidator<CreateTrainerCommand>
    {
        public CreateTrainerCommandValidator()
        {
            RuleFor(x => x.TrainerName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .MaximumLength(100)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Phone)
                .MaximumLength(20);
        }
    }
}