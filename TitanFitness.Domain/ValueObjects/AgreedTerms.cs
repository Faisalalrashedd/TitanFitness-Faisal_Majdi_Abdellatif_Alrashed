using TitanFitness.Domain.Enums;

namespace TitanFitness.Domain.ValueObjects;

public class AgreedTerms
{
    public decimal PricePaid { get; set; }

    public int DurationInMonths { get; set; }

    public int MaximumFreezeDays { get; set; }

    public int MaximumNumberOfFreezes { get; set; }

    public int GuestPassQuota { get; set; }

    public AccessScope AccessScope { get; set; }
}