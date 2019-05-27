using System;

namespace Barista.Consistency.Models
{
    public class Sale
    {
        public Guid Id { get; set; }
        public Guid OfferId { get; set; }
        public string State { get; set; }
        public decimal Quantity { get; set; }
    }
}
