using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;
using TitanFitness.Domain.Enums;
using TitanFitness.Domain.ValueObjects;

namespace TitanFitness.Application.Memberships.Commands.ChangeMembershipPlan
{
    public class ChangeMembershipPlanCommandHandler
    {
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeMembershipPlanCommandHandler(
            IGenericRepository<Membership> membershipRepository,
            IGenericRepository<Plan> planRepository,
            IUnitOfWork unitOfWork)
        {
            _membershipRepository = membershipRepository;
            _planRepository = planRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ChangeMembershipPlanResult> Handle(
            ChangeMembershipPlanCommand command)
        {
            var currentMembership =
                await _membershipRepository.GetByIdAsync(command.MembershipId);

            if (currentMembership == null)
            {
                return new ChangeMembershipPlanResult
                {
                    Error = "Membership not found"
                };
            }

            if (currentMembership.Status == MembershipStatus.Cancelled)
            {
                return new ChangeMembershipPlanResult
                {
                    Error = "Cancelled memberships cannot be changed"
                };
            }

            var plan = await _planRepository.GetByIdAsync(command.PlanId);

            if (plan == null)
            {
                return new ChangeMembershipPlanResult
                {
                    Error = "Plan not found"
                };
            }

            var startDate = currentMembership.EndDate.AddDays(1);
            var endDate = startDate
                .AddMonths(plan.DurationInMonths)
                .AddDays(-1);

            var overlap = await _membershipRepository.AnyAsync(x =>
                x.MemberId == currentMembership.MemberId &&
                x.MembershipId != currentMembership.MembershipId &&
                x.StartDate <= endDate &&
                x.EndDate >= startDate);

            if (overlap)
            {
                return new ChangeMembershipPlanResult
                {
                    Error = "New membership dates overlap another membership"
                };
            }

            var today = DateOnly.FromDateTime(DateTime.Today);

            var newMembership = new Membership
            {
                MemberId = currentMembership.MemberId,
                PlanId = plan.PlanId,
                PurchaseDate = DateTime.Now,
                StartDate = startDate,
                EndDate = endDate,
                Status = startDate > today
                    ? MembershipStatus.Pending
                    : MembershipStatus.Active,
                AgreedTerms = new AgreedTerms
                {
                    PricePaid = plan.Price,
                    DurationInMonths = plan.DurationInMonths,
                    MaximumFreezeDays = plan.MaximumFreezeDays,
                    MaximumNumberOfFreezes = plan.MaximumNumberOfFreezes,
                    GuestPassQuota = plan.GuestPassQuota,
                    AccessScope = plan.AccessScope
                }
            };

            await _membershipRepository.AddAsync(newMembership);
            await _unitOfWork.SaveChangesAsync();

            return new ChangeMembershipPlanResult
            {
                Success = true,
                MembershipId = newMembership.MembershipId
            };
        }
    }
}
