using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;
using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.Sessions.Commands.CreateSession
{
    public class CreateSessionCommandHandler
    {
        private readonly IGenericRepository<Branch> _branchRepository;
        private readonly IGenericRepository<Studio> _studioRepository;
        private readonly IGenericRepository<Trainer> _trainerRepository;
        private readonly IGenericRepository<ClassSession> _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSessionCommandHandler(
            IGenericRepository<Branch> branchRepository,
            IGenericRepository<Studio> studioRepository,
            IGenericRepository<Trainer> trainerRepository,
            IGenericRepository<ClassSession> sessionRepository,
            IUnitOfWork unitOfWork)
        {
            _branchRepository = branchRepository;
            _studioRepository = studioRepository;
            _trainerRepository = trainerRepository;
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateSessionResult> Handle(CreateSessionCommand command)
        {
            var branch = await _branchRepository.GetByIdAsync(command.BranchId);
            var studio = await _studioRepository.GetByIdAsync(command.StudioId);
            var trainer = await _trainerRepository.GetByIdAsync(command.TrainerId);

            if (branch == null || studio == null || trainer == null)
            {
                return new CreateSessionResult
                {
                    Error = "Branch studio or trainer not found"
                };
            }

            if (!trainer.IsActive)
            {
                return new CreateSessionResult
                {
                    Error = "Trainer is not active"
                };
            }

            if (studio.BranchId != command.BranchId)
            {
                return new CreateSessionResult
                {
                    Error = "Studio does not belong to this branch"
                };
            }

            if (command.CapacityLimit > studio.Capacity)
            {
                return new CreateSessionResult
                {
                    Error = "Capacity limit cannot exceed studio capacity"
                };
            }

            var newStart = command.StartTime;
            var newEnd = command.StartTime.AddMinutes(command.DurationInMinutes);

            var sessions = await _sessionRepository.FindAsync(x =>
                x.SessionDate == command.SessionDate &&
                x.Status != SessionStatus.Cancelled);

            var trainerConflict = sessions.Any(x =>
                x.TrainerId == command.TrainerId &&
                x.StartTime < newEnd &&
                x.StartTime.AddMinutes(x.DurationInMinutes) > newStart);

            if (trainerConflict)
            {
                return new CreateSessionResult
                {
                    Error = "Trainer already has an overlapping session"
                };
            }

            var studioConflict = sessions.Any(x =>
                x.StudioId == command.StudioId &&
                x.StartTime < newEnd &&
                x.StartTime.AddMinutes(x.DurationInMinutes) > newStart);

            if (studioConflict)
            {
                return new CreateSessionResult
                {
                    Error = "Studio already has an overlapping session"
                };
            }

            var session = new ClassSession
            {
                ClassName = command.ClassName,
                BranchId = command.BranchId,
                StudioId = command.StudioId,
                TrainerId = command.TrainerId,
                SessionDate = command.SessionDate,
                StartTime = command.StartTime,
                DurationInMinutes = command.DurationInMinutes,
                CapacityLimit = command.CapacityLimit,
                Status = SessionStatus.Open,
                Description = command.Description
            };

            await _sessionRepository.AddAsync(session);
            await _unitOfWork.SaveChangesAsync();

            return new CreateSessionResult
            {
                Success = true,
                SessionId = session.SessionId
            };
        }
    }
}
