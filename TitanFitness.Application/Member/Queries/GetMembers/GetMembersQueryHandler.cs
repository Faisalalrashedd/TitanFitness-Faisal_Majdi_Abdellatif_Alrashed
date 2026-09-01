using TitanFitness.Application.Interfaces;
using TitanFitness.Application.Members.Dtos;
using MemberEntity = TitanFitness.Domain.Entities.Member;

namespace TitanFitness.Application.Members.Queries.GetMembers
{
    public class GetMembersQueryHandler
    {
        private readonly IGenericRepository<MemberEntity> _memberRepository;

        public GetMembersQueryHandler(
            IGenericRepository<MemberEntity> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<List<MemberDto>> Handle(GetMembersQuery query)
        {
            var members = await _memberRepository.GetAllAsync();

            return members.Select(member => new MemberDto
            {
                MemberId = member.MemberId,
                MembershipNumber = member.MembershipNumber,
                FullName = member.FullName,
                Email = member.Email,
                Phone = member.Phone,
                Address = member.Address,
                JoinedDate = member.JoinedDate,
                Photo = member.Photo,
                HomeBranchId = member.HomeBranchId
            }).ToList();
        }
    }
}