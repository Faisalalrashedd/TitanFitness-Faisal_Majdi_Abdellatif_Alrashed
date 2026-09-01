using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.Memberships.Dtos
{
    public class MembershipDto
    {
        public int MembershipId { get; set; }
        public int MemberId { get; set; }
        public int PlanId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public MembershipStatus Status { get; set; }
        public decimal PricePaid { get; set; }
        public int DurationInMonths { get; set; }
        public int MaximumFreezeDays { get; set; }
        public int MaximumNumberOfFreezes { get; set; }
        public int GuestPassQuota { get; set; }
        public AccessScope AccessScope { get; set; }
    }
}
