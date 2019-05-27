using System;

namespace Barista.Api.Models.Accounting
{
    public class UserBalance
    {
        public Guid UserId { get; set; }
        public decimal Value { get; set; }
        public DateTimeOffset AsOf { get; set; }
    }
}
