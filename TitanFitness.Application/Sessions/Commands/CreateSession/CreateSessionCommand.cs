namespace TitanFitness.Application.Sessions.Commands.CreateSession
{
    public class CreateSessionCommand
    {
        public string ClassName { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public int StudioId { get; set; }
        public int TrainerId { get; set; }
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public int DurationInMinutes { get; set; }
        public int CapacityLimit { get; set; }
        public string? Description { get; set; }
    }
}
