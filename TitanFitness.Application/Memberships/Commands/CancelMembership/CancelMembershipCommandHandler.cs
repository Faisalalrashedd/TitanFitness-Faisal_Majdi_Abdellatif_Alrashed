using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;
using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.Memberships.Commands.CancelMembership
{
    public class CancelMembershipCommandHandler
    {
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelMembershipCommandHandler(
            IGenericRepository<Membership> membershipRepository,
            IUnitOfWork unitOfWork)
        {
            _membershipRepository = membershipRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CancelMembershipCommand command)
        {
            var membership =
                await _membershipRepository.GetByIdAsync(command.MembershipId);

            if (membership == null ||
                membership.Status == MembershipStatus.Cancelled)
            {
                return false;
            }

            membership.Status = MembershipStatus.Cancelled;

            _membershipRepository.Update(membership);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
