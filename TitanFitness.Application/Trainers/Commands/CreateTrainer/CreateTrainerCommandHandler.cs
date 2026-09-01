using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Trainers.Commands.CreateTrainer
{
    public class CreateTrainerCommandHandler
    {
        private readonly IGenericRepository<Trainer> _trainerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTrainerCommandHandler(
            IGenericRepository<Trainer> trainerRepository,
            IUnitOfWork unitOfWork)
        {
            _trainerRepository = trainerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateTrainerCommand command)
        {
            var trainer = new Trainer
            {
                TrainerName = command.TrainerName,
                Email = command.Email,
                Phone = command.Phone,
                IsActive = command.IsActive
            };

            await _trainerRepository.AddAsync(trainer);
            await _unitOfWork.SaveChangesAsync();

            return trainer.TrainerId;
        }
    }
}