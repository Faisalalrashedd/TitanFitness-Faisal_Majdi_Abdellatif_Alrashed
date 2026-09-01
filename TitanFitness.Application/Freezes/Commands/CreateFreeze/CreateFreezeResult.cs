namespace TitanFitness.Application.Freezes.Commands.CreateFreeze
{
    public class CreateFreezeResult
    {
        public bool Success { get; set; }
        public int? FreezeId { get; set; }
        public string? Error { get; set; }
    }
}
