using TitanFitness.Domain.Enums;

namespace TitanFitness.Application.CheckIns.Commands.CreateCheckIn
{
    public class CheckInResultDto
    {
        public int CheckInId { get; set; }
        public CheckInResult Result { get; set; }
        public string? RefusalReason { get; set; }
    }
}
