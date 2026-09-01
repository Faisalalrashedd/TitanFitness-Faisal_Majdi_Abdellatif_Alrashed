using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;
using TitanFitness.Domain.Enums;
using TitanFitness.Domain.ValueObjects;

namespace TitanFitness.Application.Memberships.Commands.RenewMembership
{
    public class RenewMembershipCommandHandler
    {
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RenewMembershipCommandHandler(
            IGenericRepository<Membership> membershipRepository,
            IGenericRepository<Plan> planRepository,
            IUnitOfWork unitOfWork)
        {
            _membershipRepository = membershipRepository;
            _planRepository = planRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RenewMembershipResult> Handle(
            RenewMembershipCommand command)
        {
            var currentMembership =
                await _membershipRepository.GetByIdAsync(command.MembershipId);

            if (currentMembership == null)
            {
                return new RenewMembershipResult
                {
                    Error = "Membership not found"
                };
            }

            if (currentMembership.Status == MembershipStatus.Cancelled)
            {
                return new RenewMembershipResult
                {
                    Error = "Cancelled memberships cannot be renewed from"
                };
            }

            var plan = await _planRepository.GetByIdAsync(command.PlanId);

            if (plan == null)
            {
                return new RenewMembershipResult
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
                return new RenewMembershipResult
                {
                    Error = "Renewal dates overlap another membership"
                };
            }

            var today = DateOnly.FromDateTime(DateTime.Today);

            var renewal = new Membership
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

            await _membershipRepository.AddAsync(renewal);
            await _unitOfWork.SaveChangesAsync();

            return new RenewMembershipResult
            {
                Success = true,
                MembershipId = renewal.MembershipId
            };
        }
    }
}
