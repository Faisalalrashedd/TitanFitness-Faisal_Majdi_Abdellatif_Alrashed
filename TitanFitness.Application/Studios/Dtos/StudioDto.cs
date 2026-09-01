namespace TitanFitness.Application.Studios.Dtos
{
    public class StudioDto
    {
        public int StudioId { get; set; }
        public string StudioName { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public int Capacity { get; set; }
    }
}
