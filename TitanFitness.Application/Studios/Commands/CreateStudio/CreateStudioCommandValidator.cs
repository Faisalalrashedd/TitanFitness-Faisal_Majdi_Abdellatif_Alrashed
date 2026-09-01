using FluentValidation;

namespace TitanFitness.Application.Studios.Commands.CreateStudio
{
    public class CreateStudioCommandValidator : AbstractValidator<CreateStudioCommand>
    {
        public CreateStudioCommandValidator()
        {
            RuleFor(x => x.StudioName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.BranchId)
                .GreaterThan(0);

            RuleFor(x => x.Capacity)
                .GreaterThan(0);
        }
    }
}
