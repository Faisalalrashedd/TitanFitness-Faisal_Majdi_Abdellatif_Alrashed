using FluentValidation;

namespace TitanFitness.Application.Memberships.Commands.RenewMembership
{
    public class RenewMembershipCommandValidator : AbstractValidator<RenewMembershipCommand>
    {
        public RenewMembershipCommandValidator()
        {
            RuleFor(x => x.MembershipId)
                .GreaterThan(0);

            RuleFor(x => x.PlanId)
                .GreaterThan(0);
        }
    }
}
