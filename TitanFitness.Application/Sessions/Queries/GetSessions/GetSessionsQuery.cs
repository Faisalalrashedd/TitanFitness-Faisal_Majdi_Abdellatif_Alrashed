namespace TitanFitness.Application.Sessions.Queries.GetSessions
{
    public class GetSessionsQuery
    {
        public int? BranchId { get; set; }
        public DateOnly? Date { get; set; }
    }
}
