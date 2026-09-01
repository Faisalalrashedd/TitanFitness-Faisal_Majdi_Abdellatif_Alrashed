using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;
using MemberEntity = TitanFitness.Domain.Entities.Member;
using TitanFitness.Domain.Enums;
using TitanFitness.Domain.ValueObjects;

namespace TitanFitness.Application.Memberships.Commands.PurchaseMembership
{
    public class PurchaseMembershipCommandHandler
    {
        private readonly IGenericRepository<MemberEntity> _memberRepository;
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseMembershipCommandHandler(
            IGenericRepository<MemberEntity> memberRepository,
            IGenericRepository<Plan> planRepository,
            IGenericRepository<Membership> membershipRepository,
            IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _planRepository = planRepository;
            _membershipRepository = membershipRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PurchaseMembershipResult> Handle(
            PurchaseMembershipCommand command)
        {
            var member = await _memberRepository.GetByIdAsync(command.MemberId);

            if (member == null)
            {
                return new PurchaseMembershipResult
                {
                    Error = "Member not found"
                };
            }

            var plan = await _planRepository.GetByIdAsync(command.PlanId);

            if (plan == null)
            {
                return new PurchaseMembershipResult
                {
                    Error = "Plan not found"
                };
            }

            var endDate = command.StartDate
                .AddMonths(plan.DurationInMonths)
                .AddDays(-1);

            var overlapping = await _membershipRepository.AnyAsync(x =>
                x.MemberId == command.MemberId &&
                x.StartDate <= endDate &&
                x.EndDate >= command.StartDate);

            if (overlapping)
            {
                return new PurchaseMembershipResult
                {
                    Error = "Member already has a membership covering these dates"
                };
            }

            var today = DateOnly.FromDateTime(DateTime.Today);

            var membership = new Membership
            {
                MemberId = command.MemberId,
                PlanId = command.PlanId,
                PurchaseDate = DateTime.Now,
                StartDate = command.StartDate,
                EndDate = endDate,
                Status = command.StartDate > today
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

            await _membershipRepository.AddAsync(membership);
            await _unitOfWork.SaveChangesAsync();

            return new PurchaseMembershipResult
            {
                Success = true,
                MembershipId = membership.MembershipId
            };
        }
    }
}
