using System;

namespace Barista.Accounting.Domain
{
    public class Balance
    {
        public Guid UserId { get; private set; }
        public decimal Value { get; private set; }
        public DateTimeOffset AsOf { get; private set; }

        public Balance(Guid userId, decimal value)
        {
            UserId = userId;
            Value = value;
            AsOf = DateTimeOffset.UtcNow;
        }
    }
}
