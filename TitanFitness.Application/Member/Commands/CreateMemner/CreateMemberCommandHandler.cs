using TitanFitness.Application.Interfaces;
using MemberEntity = TitanFitness.Domain.Entities.Member;
namespace TitanFitness.Application.Members.Commands.CreateMember
{
    public class CreateMemberCommandHandler
    {
        private readonly IGenericRepository<MemberEntity> _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMemberCommandHandler(
            IGenericRepository<MemberEntity> memberRepository,
            IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int?> Handle(CreateMemberCommand command)
        {
            var exists = await _memberRepository.AnyAsync(
                x => x.MembershipNumber == command.MembershipNumber);

            if (exists)
            {
                return null;
            }

            var member = new MemberEntity
            {
                MembershipNumber = command.MembershipNumber,
                FullName = command.FullName,
                Email = command.Email,
                Phone = command.Phone,
                Address = command.Address,
                JoinedDate = command.JoinedDate,
                Photo = command.Photo,
                HomeBranchId = command.HomeBranchId
            };

            await _memberRepository.AddAsync(member);
            await _unitOfWork.SaveChangesAsync();

            return member.MemberId;
        }
    }
}