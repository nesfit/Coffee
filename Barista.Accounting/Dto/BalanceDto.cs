using System;

namespace Barista.Accounting.Dto
{
    public class BalanceDto
    {
        public Guid UserId { get; set; }
        public decimal Value { get; set; }
        public DateTimeOffset AsOf { get; set; }
    }
}
