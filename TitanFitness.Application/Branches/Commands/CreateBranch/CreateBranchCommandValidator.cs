using FluentValidation;

namespace TitanFitness.Application.Branches.Commands.CreateBranch
{
    public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
    {
        public CreateBranchCommandValidator()
        {
            RuleFor(x => x.BranchName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Address)
                .MaximumLength(200);
        }
    }
}
