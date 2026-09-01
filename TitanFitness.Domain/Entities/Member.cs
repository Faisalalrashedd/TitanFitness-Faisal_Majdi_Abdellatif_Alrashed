namespace TitanFitness.Domain.Entities
{
    public class Member
    {
        public int MemberId { get; set; }

        public string MembershipNumber { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateOnly JoinedDate { get; set; }

        public byte[]? Photo { get; set; }

        public int HomeBranchId { get; set; }

        public Branch HomeBranch { get; set; } = null!;

        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();

        public ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}