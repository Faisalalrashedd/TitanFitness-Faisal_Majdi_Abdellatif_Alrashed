using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Plans.Commands.CreatePlan
{
    public class CreatePlanCommandHandler
    {
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePlanCommandHandler(
            IGenericRepository<Plan> planRepository,
            IUnitOfWork unitOfWork)
        {
            _planRepository = planRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreatePlanCommand command)
        {
            var plan = new Plan
            {
                PlanName = command.PlanName,
                Price = command.Price,
                DurationInMonths = command.DurationInMonths,
                MaximumFreezeDays = command.MaximumFreezeDays,
                MaximumNumberOfFreezes = command.MaximumNumberOfFreezes,
                GuestPassQuota = command.GuestPassQuota,
                AccessScope = command.AccessScope,
                IsPublished = command.IsPublished
            };

            await _planRepository.AddAsync(plan);
            await _unitOfWork.SaveChangesAsync();

            return plan.PlanId;
        }
    }
}

/*
CreatePlanCommand
        to
CreatePlanCommandHandler
        to
IGenericRepository<Plan>
        to
UnitOfWork
        to
SQL Server
*/
