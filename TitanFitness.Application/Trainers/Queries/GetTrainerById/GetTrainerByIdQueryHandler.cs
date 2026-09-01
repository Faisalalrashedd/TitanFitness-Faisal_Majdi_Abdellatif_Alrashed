using TitanFitness.Application.Interfaces;
using TitanFitness.Application.Trainers.Dtos;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Trainers.Queries.GetTrainerById
{
    public class GetTrainerByIdQueryHandler
    {
        private readonly IGenericRepository<Trainer> _trainerRepository;

        public GetTrainerByIdQueryHandler(
            IGenericRepository<Trainer> trainerRepository)
        {
            _trainerRepository = trainerRepository;
        }

        public async Task<TrainerDto?> Handle(GetTrainerByIdQuery query)
        {
            var trainer =
                await _trainerRepository.GetByIdAsync(query.TrainerId);

            if (trainer == null)
            {
                return null;
            }

            return new TrainerDto
            {
                TrainerId = trainer.TrainerId,
                TrainerName = trainer.TrainerName,
                Email = trainer.Email,
                Phone = trainer.Phone,
                IsActive = trainer.IsActive
            };
        }
    }
}