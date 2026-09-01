using FluentValidation;

namespace TitanFitness.Application.Trainers.Commands.UpdateTrainer
{
    public class UpdateTrainerCommandValidator
        : AbstractValidator<UpdateTrainerCommand>
    {
        public UpdateTrainerCommandValidator()
        {
            RuleFor(x => x.TrainerId)
                .GreaterThan(0);

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