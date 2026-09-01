using TitanFitness.Application.Dashboard.Dtos;
using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;
using TitanFitness.Domain.Enums;
using MemberEntity = TitanFitness.Domain.Entities.Member;

namespace TitanFitness.Application.Dashboard.Queries.GetDashboard
{
    public class GetDashboardQueryHandler
    {
        private readonly IGenericRepository<MemberEntity> _memberRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IGenericRepository<CheckIn> _checkInRepository;
        private readonly IGenericRepository<ClassSession> _sessionRepository;
        private readonly IGenericRepository<Trainer> _trainerRepository;

        public GetDashboardQueryHandler(
            IGenericRepository<MemberEntity> memberRepository,
            IGenericRepository<Membership> membershipRepository,
            IGenericRepository<CheckIn> checkInRepository,
            IGenericRepository<ClassSession> sessionRepository,
            IGenericRepository<Trainer> trainerRepository)
        {
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
            _checkInRepository = checkInRepository;
            _sessionRepository = sessionRepository;
            _trainerRepository = trainerRepository;
        }

        public async Task<DashboardDto> Handle(GetDashboardQuery query)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var members = await _memberRepository.GetAllAsync();
            var memberships = await _membershipRepository.GetAllAsync();
            var checkIns = await _checkInRepository.GetAllAsync();
            var sessions = await _sessionRepository.GetAllAsync();
            var trainers = await _trainerRepository.GetAllAsync();

            return new DashboardDto
            {
                TotalMembers = members.Count,

                ActiveMemberships = memberships.Count(x =>
                    x.StartDate <= today &&
                    x.EndDate >= today &&
                    x.Status != MembershipStatus.Cancelled &&
                    x.Status != MembershipStatus.Expired),

                TodayCheckIns = checkIns.Count(x =>
                    DateOnly.FromDateTime(x.CheckInDateTime) == today),

                UpcomingSessions = sessions.Count(x =>
                    x.SessionDate >= today &&
                    x.Status != SessionStatus.Cancelled &&
                    x.Status != SessionStatus.Completed),

                ActiveTrainers = trainers.Count(x => x.IsActive)
            };
        }
    }
}
