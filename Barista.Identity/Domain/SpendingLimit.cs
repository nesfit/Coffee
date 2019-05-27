using System;
using Barista.Common;

namespace Barista.Identity.Domain
{
    public class SpendingLimit : Entity
    {
        public SpendingLimit(Guid id, TimeSpan interval, decimal value) : base(id)
        {
            SetInterval(interval);
            SetValue(value);
        }

        public TimeSpan Interval { get; private set; }
        public decimal Value { get; private set; }

        public void SetInterval(TimeSpan interval)
        {
            if (interval.Ticks < 0)
                throw new BaristaException("invalid_interval", "The interval cannot be negative.");

            Interval = interval;
            SetUpdatedNow();
        }

        public void SetValue(decimal value)
        {
            if (value < 0)
                throw new BaristaException("invalid_value", "The value cannot be negative.");

            Value = value;
            SetUpdatedNow();
        }
    }
}
