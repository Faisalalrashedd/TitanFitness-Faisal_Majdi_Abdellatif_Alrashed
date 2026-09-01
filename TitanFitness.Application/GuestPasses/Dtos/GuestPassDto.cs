namespace TitanFitness.Application.GuestPasses.Dtos
{
    public class GuestPassDto
    {
        public int GuestPassId { get; set; }
        public int MembershipId { get; set; }
        public DateOnly IssuedOn { get; set; }
        public DateOnly? UsedOn { get; set; }
        public string? GuestName { get; set; }
    }
}
