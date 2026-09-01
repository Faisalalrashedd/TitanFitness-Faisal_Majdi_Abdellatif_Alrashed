using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.Freezes.Commands.CreateFreeze
{
    public class CreateFreezeCommand
    {
        public int MembershipId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public FreezeReason Reason { get; set; }
        public string? AdditionalNotes { get; set; }
    }
}
