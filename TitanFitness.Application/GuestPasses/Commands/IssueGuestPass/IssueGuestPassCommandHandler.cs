using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;
using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.GuestPasses.Commands.IssueGuestPass
{
    public class IssueGuestPassCommandHandler
    {
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IGenericRepository<GuestPass> _guestPassRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IssueGuestPassCommandHandler(
            IGenericRepository<Membership> membershipRepository,
            IGenericRepository<GuestPass> guestPassRepository,
            IUnitOfWork unitOfWork)
        {
            _membershipRepository = membershipRepository;
            _guestPassRepository = guestPassRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IssueGuestPassResult> Handle(
            IssueGuestPassCommand command)
        {
            var membership =
                await _membershipRepository.GetByIdAsync(command.MembershipId);

            if (membership == null)
            {
                return new IssueGuestPassResult
                {
                    Error = "Membership not found"
                };
            }

            var today = DateOnly.FromDateTime(DateTime.Today);

            if (membership.Status == MembershipStatus.Cancelled ||
                membership.Status == MembershipStatus.Expired ||
                today < membership.StartDate ||
                today > membership.EndDate)
            {
                return new IssueGuestPassResult
                {
                    Error = "Membership is not active"
                };
            }

            var existingPasses = await _guestPassRepository.FindAsync(
                x => x.MembershipId == command.MembershipId);

            if (existingPasses.Count >= membership.AgreedTerms.GuestPassQuota)
            {
                return new IssueGuestPassResult
                {
                    Error = "Guest pass quota has been reached"
                };
            }

            var guestPass = new GuestPass
            {
                MembershipId = command.MembershipId,
                IssuedOn = today,
                GuestName = command.GuestName
            };

            await _guestPassRepository.AddAsync(guestPass);
            await _unitOfWork.SaveChangesAsync();

            return new IssueGuestPassResult
            {
                Success = true,
                GuestPassId = guestPass.GuestPassId
            };
        }
    }
}
