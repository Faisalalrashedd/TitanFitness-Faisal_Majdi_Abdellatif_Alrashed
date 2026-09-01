namespace TitanFitness.Application.Trainers.Commands.CreateTrainer
{
    public class CreateTrainerCommand
    {
        public string TrainerName { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public bool IsActive { get; set; }
    }
}