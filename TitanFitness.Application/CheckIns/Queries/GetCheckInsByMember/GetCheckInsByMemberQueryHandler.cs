using TitanFitness.Application.CheckIns.Dtos;
using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.CheckIns.Queries.GetCheckInsByMember
{
    public class GetCheckInsByMemberQueryHandler
    {
        private readonly IGenericRepository<CheckIn> _checkInRepository;

        public GetCheckInsByMemberQueryHandler(
            IGenericRepository<CheckIn> checkInRepository)
        {
            _checkInRepository = checkInRepository;
        }

        public async Task<List<CheckInDto>> Handle(
            GetCheckInsByMemberQuery query)
        {
            var checkIns = await _checkInRepository.FindAsync(
                x => x.MemberId == query.MemberId);

            return checkIns
                .OrderByDescending(x => x.CheckInDateTime)
                .Select(x => new CheckInDto
                {
                    CheckInId = x.CheckInId,
                    MemberId = x.MemberId,
                    BranchId = x.BranchId,
                    CheckInDateTime = x.CheckInDateTime,
                    Result = x.Result,
                    RefusalReason = x.RefusalReason
                })
                .ToList();
        }
    }
}
