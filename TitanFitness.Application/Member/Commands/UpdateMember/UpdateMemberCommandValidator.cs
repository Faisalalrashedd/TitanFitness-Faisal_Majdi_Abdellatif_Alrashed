using FluentValidation;

namespace TitanFitness.Application.Members.Commands.UpdateMember
{
    public class UpdateMemberCommandValidator
        : AbstractValidator<UpdateMemberCommand>
    {
        public UpdateMemberCommandValidator()
        {
            RuleFor(x => x.MemberId)
                .GreaterThan(0);

            RuleFor(x => x.MembershipNumber)
                .NotEmpty()
                .MaximumLength(10);

            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .MaximumLength(100)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Phone)
                .MaximumLength(20);

            RuleFor(x => x.Address)
                .MaximumLength(200);

            RuleFor(x => x.JoinedDate)
                .NotEmpty();

            RuleFor(x => x.HomeBranchId)
                .GreaterThan(0);
        }
    }
}