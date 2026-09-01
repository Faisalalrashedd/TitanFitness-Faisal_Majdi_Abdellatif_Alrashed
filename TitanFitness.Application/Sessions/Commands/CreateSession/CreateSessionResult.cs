namespace TitanFitness.Application.Sessions.Commands.CreateSession
{
    public class CreateSessionResult
    {
        public bool Success { get; set; }
        public int? SessionId { get; set; }
        public string? Error { get; set; }
    }
}
