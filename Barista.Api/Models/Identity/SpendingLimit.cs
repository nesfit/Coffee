using System;

namespace Barista.Api.Models.Identity
{
    public class SpendingLimit
    {
        public Guid Id { get; set; }
        public TimeSpan Interval { get; set; }
        public decimal Value { get; set; }

        public SpendingLimit(SpendingLimit spendingLimit)
        {
            Id = spendingLimit.Id;
            Interval = spendingLimit.Interval;
            Value = spendingLimit.Value;
        }
    }
}
