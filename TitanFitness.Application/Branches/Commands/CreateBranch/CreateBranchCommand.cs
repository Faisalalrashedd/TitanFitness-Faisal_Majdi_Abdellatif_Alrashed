namespace TitanFitness.Application.Branches.Commands.CreateBranch
{
    public class CreateBranchCommand
    {
        public string BranchName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public TimeOnly? OpeningTime { get; set; }
        public TimeOnly? ClosingTime { get; set; }
    }
}
