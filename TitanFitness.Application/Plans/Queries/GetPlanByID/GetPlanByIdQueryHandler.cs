using TitanFitness.Application.Interfaces;
using TitanFitness.Application.Plans.Dtos;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Plans.Queries.GetPlanByID
{
    public class GetPlanByIdQueryHandler
    {
        private readonly IGenericRepository<Plan> _planRepository;

        public GetPlanByIdQueryHandler(IGenericRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<PlanDto?> Handle(GetPlanByIdQuery query)
        {
            var plan = await _planRepository.GetByIdAsync(query.PlanId);

            if (plan == null)
            {
                return null;
            }

            return new PlanDto
            {
                PlanId = plan.PlanId,
                PlanName = plan.PlanName,
                Price = plan.Price,
                DurationInMonths = plan.DurationInMonths,
                MaximumFreezeDays = plan.MaximumFreezeDays,
                MaximumNumberOfFreezes = plan.MaximumNumberOfFreezes,
                GuestPassQuota = plan.GuestPassQuota,
                AccessScope = plan.AccessScope,
                IsPublished = plan.IsPublished
            };
        }
    }
}