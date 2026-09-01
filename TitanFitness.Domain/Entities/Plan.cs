using TitanFitness.Domain.Enums;

namespace TitanFitness.Domain.Entities
{
    public class Plan
    {
        public int PlanId { get; set; }

        public string PlanName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int DurationInMonths { get; set; }

        public int MaximumFreezeDays { get; set; }

        public int MaximumNumberOfFreezes { get; set; }

        public int GuestPassQuota { get; set; }

        public AccessScope AccessScope { get; set; }

        public bool IsPublished { get; set; }

        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    }
}