using System;

namespace Barista.Identity.Dto
{
    public class SpendingLimitDto
    {
        public Guid Id { get; set; }
        public TimeSpan Interval { get; set; }
        public decimal Value { get; set; }
    }
}
