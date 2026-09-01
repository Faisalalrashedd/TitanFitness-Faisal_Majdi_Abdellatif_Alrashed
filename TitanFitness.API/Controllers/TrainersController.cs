using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.Trainers.Commands.CreateTrainer;
using TitanFitness.Application.Trainers.Commands.UpdateTrainer;
using TitanFitness.Application.Trainers.Queries.GetTrainerById;
using TitanFitness.Application.Trainers.Queries.GetTrainers;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainersController : ControllerBase
    {
        private readonly CreateTrainerCommandHandler _createTrainerHandler;
        private readonly GetTrainersQueryHandler _getTrainersHandler;
        private readonly GetTrainerByIdQueryHandler _getTrainerByIdHandler;
        private readonly UpdateTrainerCommandHandler _updateTrainerHandler;
        private readonly IValidator<CreateTrainerCommand> _createTrainerValidator;
        private readonly IValidator<UpdateTrainerCommand> _updateTrainerValidator;

        public TrainersController(
            CreateTrainerCommandHandler createTrainerHandler,
            GetTrainersQueryHandler getTrainersHandler,
            GetTrainerByIdQueryHandler getTrainerByIdHandler,
            UpdateTrainerCommandHandler updateTrainerHandler,
            IValidator<CreateTrainerCommand> createTrainerValidator,
            IValidator<UpdateTrainerCommand> updateTrainerValidator)
        {
            _createTrainerHandler = createTrainerHandler;
            _getTrainersHandler = getTrainersHandler;
            _getTrainerByIdHandler = getTrainerByIdHandler;
            _updateTrainerHandler = updateTrainerHandler;
            _createTrainerValidator = createTrainerValidator;
            _updateTrainerValidator = updateTrainerValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrainers()
        {
            var trainers =
                await _getTrainersHandler.Handle(new GetTrainersQuery());

            return Ok(trainers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainerById(int id)
        {
            var trainer =
                await _getTrainerByIdHandler.Handle(new GetTrainerByIdQuery
                {
                    TrainerId = id
                });

            if (trainer == null)
            {
                return NotFound();
            }

            return Ok(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrainer(
            CreateTrainerCommand command)
        {
            var validationResult =
                await _createTrainerValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var trainerId =
                await _createTrainerHandler.Handle(command);

            return Ok(new
            {
                TrainerId = trainerId,
                Message = "Trainer created successfully"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainer(
            int id,
            UpdateTrainerCommand command)
        {
            command.TrainerId = id;

            var validationResult =
                await _updateTrainerValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var updated =
                await _updateTrainerHandler.Handle(command);

            if (!updated)
            {
                return NotFound();
            }

            return Ok(new
            {
                Message = "Trainer updated successfully"
            });
        }
    }
}