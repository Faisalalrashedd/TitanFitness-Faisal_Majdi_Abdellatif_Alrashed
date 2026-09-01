namespace TitanFitness.Application.Memberships.Commands.PurchaseMembership
{
    public class PurchaseMembershipCommand
    {
        public int MemberId { get; set; }
        public int PlanId { get; set; }
        public DateOnly StartDate { get; set; }
    }
}
