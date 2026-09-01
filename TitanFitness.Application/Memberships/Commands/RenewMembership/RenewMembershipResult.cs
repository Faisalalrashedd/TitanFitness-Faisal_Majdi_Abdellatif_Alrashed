namespace TitanFitness.Application.Memberships.Commands.RenewMembership
{
    public class RenewMembershipResult
    {
        public bool Success { get; set; }
        public int? MembershipId { get; set; }
        public string? Error { get; set; }
    }
}
