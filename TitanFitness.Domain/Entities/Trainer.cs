namespace TitanFitness.Domain.Entities
{
    public class Trainer
    {
        public int TrainerId { get; set; }

        public string TrainerName { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public bool IsActive { get; set; }

        public ICollection<ClassSession> ClassSessions { get; set; } = new List<ClassSession>();
    }
}