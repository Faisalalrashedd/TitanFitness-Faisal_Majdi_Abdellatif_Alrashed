using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.GuestPasses.Commands.UseGuestPass
{
    public class UseGuestPassCommandHandler
    {
        private readonly IGenericRepository<GuestPass> _guestPassRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UseGuestPassCommandHandler(
            IGenericRepository<GuestPass> guestPassRepository,
            IUnitOfWork unitOfWork)
        {
            _guestPassRepository = guestPassRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UseGuestPassCommand command)
        {
            var guestPass =
                await _guestPassRepository.GetByIdAsync(command.GuestPassId);

            if (guestPass == null || guestPass.UsedOn != null)
            {
                return false;
            }

            guestPass.UsedOn = DateOnly.FromDateTime(DateTime.Today);

            _guestPassRepository.Update(guestPass);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
