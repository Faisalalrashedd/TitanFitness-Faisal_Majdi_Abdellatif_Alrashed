using TitanFitness.Domain.Enums;

namespace TitanFitness.Domain.Entities
{
    public class CheckIn
    {
        public int CheckInId { get; set; }

        public int MemberId { get; set; }

        public int BranchId { get; set; }

        public DateTime CheckInDateTime { get; set; }

        public CheckInResult Result { get; set; }

        public string? RefusalReason { get; set; }

        public Member Member { get; set; } = null!;

        public Branch Branch { get; set; } = null!;
    }
}