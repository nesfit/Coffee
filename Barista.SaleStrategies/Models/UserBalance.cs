using System;

namespace Barista.SaleStrategies.Models
{
    public class UserBalance
    {
        public Guid UserId { get; set; }
        public decimal Value { get; set; }
        public DateTimeOffset AsOf { get; set; }
    }
}
