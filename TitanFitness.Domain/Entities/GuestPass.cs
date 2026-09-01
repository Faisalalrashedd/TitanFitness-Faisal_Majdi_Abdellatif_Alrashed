namespace TitanFitness.Domain.Entities
{
    public class GuestPass
    {
        public int GuestPassId { get; set; }

        public int MembershipId { get; set; }

        public DateOnly IssuedOn { get; set; }

        public DateOnly? UsedOn { get; set; }

        public string? GuestName { get; set; }

        public Membership Membership { get; set; } = null!;
    }
}