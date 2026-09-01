using FluentValidation;

namespace TitanFitness.Application.Plans.Commands.CreatePlan
{
    public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
    {
        public CreatePlanCommandValidator()
        {
            RuleFor(x => x.PlanName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.DurationInMonths)
                .GreaterThan(0);

            RuleFor(x => x.MaximumFreezeDays)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.MaximumNumberOfFreezes)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.GuestPassQuota)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.AccessScope)
                .IsInEnum();
        }
    }
}