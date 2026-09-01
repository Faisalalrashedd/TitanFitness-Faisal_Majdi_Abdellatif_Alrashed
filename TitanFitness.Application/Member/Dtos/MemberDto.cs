namespace TitanFitness.Application.Members.Dtos
{
    public class MemberDto
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
    }
}