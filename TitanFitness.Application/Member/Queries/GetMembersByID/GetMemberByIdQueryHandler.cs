using TitanFitness.Application.Interfaces;
using TitanFitness.Application.Member.Queries.GetMembersByID;
using TitanFitness.Application.Members.Dtos;
using MemberEntity = TitanFitness.Domain.Entities.Member;


namespace TitanFitness.Application.Members.Queries.GetMemberById
{
    public class GetMemberByIdQueryHandler
    {
        private readonly IGenericRepository<MemberEntity> _memberRepository;

        public GetMemberByIdQueryHandler(
            IGenericRepository<MemberEntity> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<MemberDto?> Handle(GetMemberByIdQuery query)
        {
            var member =
                await _memberRepository.GetByIdAsync(query.MemberId);

            if (member == null)
            {
                return null;
            }

            return new MemberDto
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
            };
        }
    }
}