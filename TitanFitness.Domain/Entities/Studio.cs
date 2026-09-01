namespace TitanFitness.Domain.Entities
{
    public class Studio
    {
        public int StudioId { get; set; }

        public string StudioName { get; set; } = string.Empty;

        public int BranchId { get; set; }

        public int Capacity { get; set; }

        public Branch Branch { get; set; } = null!;
    }
}