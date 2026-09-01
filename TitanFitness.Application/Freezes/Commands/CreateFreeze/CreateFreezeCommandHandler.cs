using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;
using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.Freezes.Commands.CreateFreeze
{
    public class CreateFreezeCommandHandler
    {
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IGenericRepository<Freeze> _freezeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateFreezeCommandHandler(
            IGenericRepository<Membership> membershipRepository,
            IGenericRepository<Freeze> freezeRepository,
            IUnitOfWork unitOfWork)
        {
            _membershipRepository = membershipRepository;
            _freezeRepository = freezeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateFreezeResult> Handle(CreateFreezeCommand command)
        {
            var membership =
                await _membershipRepository.GetByIdAsync(command.MembershipId);

            if (membership == null)
            {
                return new CreateFreezeResult
                {
                    Error = "Membership not found"
                };
            }

            if (membership.Status == MembershipStatus.Cancelled ||
                membership.Status == MembershipStatus.Expired)
            {
                return new CreateFreezeResult
                {
                    Error = "This membership cannot be frozen"
                };
            }

            var today = DateOnly.FromDateTime(DateTime.Today);

            if (command.StartDate < today)
            {
                return new CreateFreezeResult
                {
                    Error = "Freeze cannot begin in the past"
                };
            }

            if (command.StartDate < membership.StartDate ||
                command.EndDate > membership.EndDate)
            {
                return new CreateFreezeResult
                {
                    Error = "Freeze dates must be inside the membership period"
                };
            }

            var existingFreezes = await _freezeRepository.FindAsync(
                x => x.MembershipId == command.MembershipId);

            if (existingFreezes.Count >=
                membership.AgreedTerms.MaximumNumberOfFreezes)
            {
                return new CreateFreezeResult
                {
                    Error = "Maximum number of freezes has been reached"
                };
            }

            var overlaps = existingFreezes.Any(x =>
                x.StartDate <= command.EndDate &&
                x.EndDate >= command.StartDate);

            if (overlaps)
            {
                return new CreateFreezeResult
                {
                    Error = "Freeze dates overlap an existing freeze"
                };
            }

            var freezeDays =
                command.EndDate.DayNumber - command.StartDate.DayNumber + 1;

            var usedFreezeDays = existingFreezes.Sum(x =>
                x.EndDate.DayNumber - x.StartDate.DayNumber + 1);

            if (usedFreezeDays + freezeDays >
                membership.AgreedTerms.MaximumFreezeDays)
            {
                return new CreateFreezeResult
                {
                    Error = "Maximum freeze days would be exceeded"
                };
            }

            var freeze = new Freeze
            {
                MembershipId = command.MembershipId,
                StartDate = command.StartDate,
                EndDate = command.EndDate,
                DurationInMonths = 0,
                Reason = command.Reason,
                AdditionalNotes = command.AdditionalNotes,
                RequestedOn = DateTime.Now
            };

            membership.EndDate = membership.EndDate.AddDays(freezeDays);

            if (command.StartDate <= today && command.EndDate >= today)
            {
                membership.Status = MembershipStatus.Frozen;
            }

            await _freezeRepository.AddAsync(freeze);
            _membershipRepository.Update(membership);
            await _unitOfWork.SaveChangesAsync();

            return new CreateFreezeResult
            {
                Success = true,
                FreezeId = freeze.FreezeId
            };
        }
    }
}
