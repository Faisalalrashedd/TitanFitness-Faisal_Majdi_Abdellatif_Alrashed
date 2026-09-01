using TitanFitness.Application.GuestPasses.Dtos;
using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.GuestPasses.Queries.GetGuestPasses
{
    public class GetGuestPassesQueryHandler
    {
        private readonly IGenericRepository<GuestPass> _guestPassRepository;

        public GetGuestPassesQueryHandler(
            IGenericRepository<GuestPass> guestPassRepository)
        {
            _guestPassRepository = guestPassRepository;
        }

        public async Task<List<GuestPassDto>> Handle(GetGuestPassesQuery query)
        {
            var passes = await _guestPassRepository.FindAsync(
                x => x.MembershipId == query.MembershipId);

            return passes.Select(x => new GuestPassDto
            {
                GuestPassId = x.GuestPassId,
                MembershipId = x.MembershipId,
                IssuedOn = x.IssuedOn,
                UsedOn = x.UsedOn,
                GuestName = x.GuestName
            }).ToList();
        }
    }
}
