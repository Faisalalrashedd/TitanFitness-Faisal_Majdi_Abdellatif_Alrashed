namespace TitanFitness.Application.Memberships.Commands.ChangeMembershipPlan
{
    public class ChangeMembershipPlanResult
    {
        public bool Success { get; set; }
        public int? MembershipId { get; set; }
        public string? Error { get; set; }
    }
}
