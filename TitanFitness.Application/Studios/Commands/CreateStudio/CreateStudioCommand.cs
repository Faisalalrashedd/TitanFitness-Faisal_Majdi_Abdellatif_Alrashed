namespace TitanFitness.Application.Studios.Commands.CreateStudio
{
    public class CreateStudioCommand
    {
        public string StudioName { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public int Capacity { get; set; }
    }
}
