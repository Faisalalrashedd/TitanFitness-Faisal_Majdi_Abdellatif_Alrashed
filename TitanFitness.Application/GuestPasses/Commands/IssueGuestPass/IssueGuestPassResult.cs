namespace TitanFitness.Application.GuestPasses.Commands.IssueGuestPass
{
    public class IssueGuestPassResult
    {
        public bool Success { get; set; }
        public int? GuestPassId { get; set; }
        public string? Error { get; set; }
    }
}
