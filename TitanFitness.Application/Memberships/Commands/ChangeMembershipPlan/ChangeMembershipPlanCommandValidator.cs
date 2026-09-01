using FluentValidation;

namespace TitanFitness.Application.Memberships.Commands.ChangeMembershipPlan
{
    public class ChangeMembershipPlanCommandValidator
        : AbstractValidator<ChangeMembershipPlanCommand>
    {
        public ChangeMembershipPlanCommandValidator()
        {
            RuleFor(x => x.MembershipId)
                .GreaterThan(0);

            RuleFor(x => x.PlanId)
                .GreaterThan(0);
        }
    }
}
