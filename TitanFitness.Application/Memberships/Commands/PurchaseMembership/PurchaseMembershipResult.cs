namespace TitanFitness.Application.Memberships.Commands.PurchaseMembership
{
    public class PurchaseMembershipResult
    {
        public bool Success { get; set; }
        public int? MembershipId { get; set; }
        public string? Error { get; set; }
    }
}
