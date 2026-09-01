using TitanFitness.Application.Interfaces;
using TitanFitness.Application.Sessions.Dtos;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Sessions.Queries.GetSessions
{
    public class GetSessionsQueryHandler
    {
        private readonly IGenericRepository<ClassSession> _sessionRepository;

        public GetSessionsQueryHandler(
            IGenericRepository<ClassSession> sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<List<SessionDto>> Handle(GetSessionsQuery query)
        {
            var sessions = await _sessionRepository.GetAllAsync();

            if (query.BranchId.HasValue)
            {
                sessions = sessions
                    .Where(x => x.BranchId == query.BranchId.Value)
                    .ToList();
            }

            if (query.Date.HasValue)
            {
                sessions = sessions
                    .Where(x => x.SessionDate == query.Date.Value)
                    .ToList();
            }

            return sessions
                .OrderBy(x => x.SessionDate)
                .ThenBy(x => x.StartTime)
                .Select(x => new SessionDto
                {
                    SessionId = x.SessionId,
                    ClassName = x.ClassName,
                    BranchId = x.BranchId,
                    StudioId = x.StudioId,
                    TrainerId = x.TrainerId,
                    SessionDate = x.SessionDate,
                    StartTime = x.StartTime,
                    DurationInMinutes = x.DurationInMinutes,
                    CapacityLimit = x.CapacityLimit,
                    Status = x.Status,
                    Description = x.Description
                })
                .ToList();
        }
    }
}
