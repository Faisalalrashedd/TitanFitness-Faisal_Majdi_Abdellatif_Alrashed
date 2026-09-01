using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.Plans.Commands.CreatePlan
{
    public class CreatePlanCommand
    {
        public string PlanName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int DurationInMonths { get; set; }

        public int MaximumFreezeDays { get; set; }

        public int MaximumNumberOfFreezes { get; set; }

        public int GuestPassQuota { get; set; }

        public AccessScope AccessScope { get; set; }

        public bool IsPublished { get; set; }
    }
}