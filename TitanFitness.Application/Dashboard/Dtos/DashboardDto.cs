namespace TitanFitness.Application.Dashboard.Dtos
{
    public class DashboardDto
    {
        public int TotalMembers { get; set; }
        public int ActiveMemberships { get; set; }
        public int TodayCheckIns { get; set; }
        public int UpcomingSessions { get; set; }
        public int ActiveTrainers { get; set; }
    }
}
