using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Trainers.Commands.UpdateTrainer
{
    public class UpdateTrainerCommandHandler
    {
        private readonly IGenericRepository<Trainer> _trainerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTrainerCommandHandler(
            IGenericRepository<Trainer> trainerRepository,
            IUnitOfWork unitOfWork)
        {
            _trainerRepository = trainerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateTrainerCommand command)
        {
            var trainer =
                await _trainerRepository.GetByIdAsync(command.TrainerId);

            if (trainer == null)
            {
                return false;
            }

            trainer.TrainerName = command.TrainerName;
            trainer.Email = command.Email;
            trainer.Phone = command.Phone;
            trainer.IsActive = command.IsActive;

            _trainerRepository.Update(trainer);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}