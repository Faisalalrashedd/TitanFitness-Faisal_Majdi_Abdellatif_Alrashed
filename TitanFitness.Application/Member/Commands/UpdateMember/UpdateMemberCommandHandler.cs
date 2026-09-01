using TitanFitness.Application.Interfaces;
using MemberEntity = TitanFitness.Domain.Entities.Member;


namespace TitanFitness.Application.Members.Commands.UpdateMember
{
    public class UpdateMemberCommandHandler
    {
        private readonly IGenericRepository<MemberEntity> _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMemberCommandHandler(
            IGenericRepository<MemberEntity> memberRepository,
            IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateMemberCommand command)
        {
            var member =
                await _memberRepository.GetByIdAsync(command.MemberId);

            if (member == null)
            {
                return false;
            }

            var duplicate = await _memberRepository.AnyAsync(
                x => x.MembershipNumber == command.MembershipNumber &&
                     x.MemberId != command.MemberId);

            if (duplicate)
            {
                return false;
            }

            member.MembershipNumber = command.MembershipNumber;
            member.FullName = command.FullName;
            member.Email = command.Email;
            member.Phone = command.Phone;
            member.Address = command.Address;
            member.JoinedDate = command.JoinedDate;
            member.Photo = command.Photo;
            member.HomeBranchId = command.HomeBranchId;

            _memberRepository.Update(member);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}