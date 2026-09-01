using TitanFitness.Application.Interfaces;
using TitanFitness.Application.Memberships.Dtos;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Memberships.Queries.GetMembershipsByMember
{
    public class GetMembershipsByMemberQueryHandler
    {
        private readonly IGenericRepository<Membership> _membershipRepository;

        public GetMembershipsByMemberQueryHandler(
            IGenericRepository<Membership> membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public async Task<List<MembershipDto>> Handle(
            GetMembershipsByMemberQuery query)
        {
            var memberships = await _membershipRepository.FindAsync(
                x => x.MemberId == query.MemberId);

            return memberships
                .OrderByDescending(x => x.StartDate)
                .Select(x => new MembershipDto
                {
                    MembershipId = x.MembershipId,
                    MemberId = x.MemberId,
                    PlanId = x.PlanId,
                    PurchaseDate = x.PurchaseDate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Status = x.Status,
                    PricePaid = x.AgreedTerms.PricePaid,
                    DurationInMonths = x.AgreedTerms.DurationInMonths,
                    MaximumFreezeDays = x.AgreedTerms.MaximumFreezeDays,
                    MaximumNumberOfFreezes = x.AgreedTerms.MaximumNumberOfFreezes,
                    GuestPassQuota = x.AgreedTerms.GuestPassQuota,
                    AccessScope = x.AgreedTerms.AccessScope
                })
                .ToList();
        }
    }
}
