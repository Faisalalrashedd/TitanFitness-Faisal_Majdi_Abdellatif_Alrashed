namespace TitanFitness.Application.GuestPasses.Commands.IssueGuestPass
{
    public class IssueGuestPassCommand
    {
        public int MembershipId { get; set; }
        public string? GuestName { get; set; }
    }
}
