using TitanFitness.Domain.Enums;

namespace TitanFitness.Domain.Entities
{
    public class ClassSession
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

        public Branch Branch { get; set; } = null!;

        public Studio Studio { get; set; } = null!;

        public Trainer Trainer { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}