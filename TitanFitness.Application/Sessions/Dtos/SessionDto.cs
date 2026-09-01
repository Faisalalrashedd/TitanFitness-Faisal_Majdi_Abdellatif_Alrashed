using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.Sessions.Dtos
{
    public class SessionDto
    {
        public int SessionId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public int StudioId { get; set; }
        public int TrainerId { get; set; }
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public int DurationInMinutes { get; set; }
        public int CapacityLimit { get; set; }
        public SessionStatus Status { get; set; }
        public string? Description { get; set; }
    }
}
