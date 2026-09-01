using TitanFitness.Domain.Enums;
using TitanFitness.Domain.ValueObjects;

namespace TitanFitness.Domain.Entities
{
    public class Membership
    {
        public int MembershipId { get; set; }

        public int MemberId { get; set; }

        public int PlanId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public MembershipStatus Status { get; set; }

        public AgreedTerms AgreedTerms { get; set; } = new();

        public Member Member { get; set; } = null!;

        public Plan Plan { get; set; } = null!;

        public ICollection<Freeze> Freezes { get; set; } = new List<Freeze>();

        public ICollection<GuestPass> GuestPasses { get; set; } = new List<GuestPass>();
    }
}