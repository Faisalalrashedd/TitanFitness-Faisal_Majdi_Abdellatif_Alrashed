using TitanFitness.Application.Interfaces;
using TitanFitness.Application.Trainers.Dtos;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Trainers.Queries.GetTrainers
{
    public class GetTrainersQueryHandler
    {
        private readonly IGenericRepository<Trainer> _trainerRepository;

        public GetTrainersQueryHandler(
            IGenericRepository<Trainer> trainerRepository)
        {
            _trainerRepository = trainerRepository;
        }

        public async Task<List<TrainerDto>> Handle(GetTrainersQuery query)
        {
            var trainers = await _trainerRepository.GetAllAsync();

            return trainers.Select(trainer => new TrainerDto
            {
                TrainerId = trainer.TrainerId,
                TrainerName = trainer.TrainerName,
                Email = trainer.Email,
                Phone = trainer.Phone,
                IsActive = trainer.IsActive
            }).ToList();
        }
    }
}