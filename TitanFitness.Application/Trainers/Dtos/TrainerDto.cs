namespace TitanFitness.Application.Trainers.Dtos
{
    public class TrainerDto
    {
        public int TrainerId { get; set; }

        public string TrainerName { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public bool IsActive { get; set; }
    }
}