using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.CheckIns.Dtos
{
    public class CheckInDto
    {
        public int CheckInId { get; set; }
        public int MemberId { get; set; }
        public int BranchId { get; set; }
        public DateTime CheckInDateTime { get; set; }
        public CheckInResult Result { get; set; }
        public string? RefusalReason { get; set; }
    }
}
