using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Plans.Commands.UpdatePlan
{
    public class UpdatePlanCommandHandler
    {
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePlanCommandHandler(
            IGenericRepository<Plan> planRepository,
            IUnitOfWork unitOfWork)
        {
            _planRepository = planRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdatePlanCommand command)
        {
            var plan = await _planRepository.GetByIdAsync(command.PlanId);

            if (plan == null)
            {
                return false;
            }

            plan.PlanName = command.PlanName;
            plan.Price = command.Price;
            plan.DurationInMonths = command.DurationInMonths;
            plan.MaximumFreezeDays = command.MaximumFreezeDays;
            plan.MaximumNumberOfFreezes = command.MaximumNumberOfFreezes;
            plan.GuestPassQuota = command.GuestPassQuota;
            plan.AccessScope = command.AccessScope;
            plan.IsPublished = command.IsPublished;

            _planRepository.Update(plan);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}