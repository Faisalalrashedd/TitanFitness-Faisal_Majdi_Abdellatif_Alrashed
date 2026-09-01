using TitanFitness.Domain.Enums;

namespace TitanFitness.Domain.Entities
{
    public class Freeze
    {
        public int FreezeId { get; set; }

        public int MembershipId { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int DurationInMonths { get; set; }

        public FreezeReason Reason { get; set; }

        public string? AdditionalNotes { get; set; }

        public DateTime RequestedOn { get; set; }

        public Membership Membership { get; set; } = null!;
    }
}