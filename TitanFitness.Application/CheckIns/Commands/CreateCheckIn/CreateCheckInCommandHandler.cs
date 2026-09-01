using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;
using TitanFitness.Domain.Enums;
using MemberEntity = TitanFitness.Domain.Entities.Member;

namespace TitanFitness.Application.CheckIns.Commands.CreateCheckIn
{
    public class CreateCheckInCommandHandler
    {
        private readonly IGenericRepository<MemberEntity> _memberRepository;
        private readonly IGenericRepository<Branch> _branchRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IGenericRepository<Freeze> _freezeRepository;
        private readonly IGenericRepository<CheckIn> _checkInRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCheckInCommandHandler(
            IGenericRepository<MemberEntity> memberRepository,
            IGenericRepository<Branch> branchRepository,
            IGenericRepository<Membership> membershipRepository,
            IGenericRepository<Freeze> freezeRepository,
            IGenericRepository<CheckIn> checkInRepository,
            IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _branchRepository = branchRepository;
            _membershipRepository = membershipRepository;
            _freezeRepository = freezeRepository;
            _checkInRepository = checkInRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CheckInResultDto?> Handle(CreateCheckInCommand command)
        {
            var member = await _memberRepository.GetByIdAsync(command.MemberId);
            var branch = await _branchRepository.GetByIdAsync(command.BranchId);

            if (member == null || branch == null)
            {
                return null;
            }

            var today = DateOnly.FromDateTime(DateTime.Today);
            var now = DateTime.Now;

            var memberships = await _membershipRepository.FindAsync(x =>
                x.MemberId == command.MemberId &&
                x.StartDate <= today &&
                x.EndDate >= today &&
                x.Status != MembershipStatus.Cancelled &&
                x.Status != MembershipStatus.Expired);

            var membership = memberships
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();

            var result = CheckInResult.Refused;
            string? refusalReason = null;

            if (membership == null)
            {
                refusalReason = "No active membership";
            }
            else
            {
                var freezes = await _freezeRepository.FindAsync(x =>
                    x.MembershipId == membership.MembershipId &&
                    x.StartDate <= today &&
                    x.EndDate >= today);

                if (freezes.Any())
                {
                    refusalReason = "Membership is frozen";
                }
                else if (membership.AgreedTerms.AccessScope ==
                         AccessScope.HomeBranchOnly &&
                         member.HomeBranchId != command.BranchId)
                {
                    refusalReason = "Membership does not allow access to this branch";
                }
                else
                {
                    result = CheckInResult.Admitted;
                }
            }

            var checkIn = new CheckIn
            {
                MemberId = command.MemberId,
                BranchId = command.BranchId,
                CheckInDateTime = now,
                Result = result,
                RefusalReason = refusalReason
            };

            await _checkInRepository.AddAsync(checkIn);
            await _unitOfWork.SaveChangesAsync();

            return new CheckInResultDto
            {
                CheckInId = checkIn.CheckInId,
                Result = result,
                RefusalReason = refusalReason
            };
        }
    }
}
