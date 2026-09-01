namespace TitanFitness.Application.Branches.Dtos
{
    public class BranchDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public TimeOnly? OpeningTime { get; set; }
        public TimeOnly? ClosingTime { get; set; }
    }
}
