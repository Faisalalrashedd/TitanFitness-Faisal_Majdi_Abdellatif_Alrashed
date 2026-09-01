namespace TitanFitness.Domain.Entities
{
    public class Branch
    {
        public int BranchId { get; set; }

        public string BranchName { get; set; } = string.Empty;

        public string? Address { get; set; }

        public TimeOnly? OpeningTime { get; set; }

        public TimeOnly? ClosingTime { get; set; }

        public ICollection<Studio> Studios { get; set; } = new List<Studio>();
    }
}