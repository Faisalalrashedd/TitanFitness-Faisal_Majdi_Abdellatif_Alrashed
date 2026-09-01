using TitanFitness.Application.Interfaces;
using TitanFitness.Application.Plans.Dtos;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Plans.Queries.GetPlans
{
    public class GetPlansQueryHandler
    {
        private readonly IGenericRepository<Plan> _planRepository;

        public GetPlansQueryHandler(IGenericRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<List<PlanDto>> Handle(GetPlansQuery query)
        {
            var plans = await _planRepository.GetAllAsync();

            return plans.Select(plan => new PlanDto
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
            }).ToList();
        }
    }
}