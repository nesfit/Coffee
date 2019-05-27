using System;

namespace Barista.Api.Models.Accounting
{
    public class Payment
    {
        public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public decimal Amount { get; set; }
        public Guid UserId { get; set; }
        public string Source { get; set; }
        public string ExternalId { get; set; }
    }
}
