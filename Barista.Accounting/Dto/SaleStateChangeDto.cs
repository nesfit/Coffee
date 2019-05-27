using System;

namespace Barista.Accounting.Dto
{
    public class SaleStateChangeDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Reason { get; set; }
        public string State { get; set; }
        public Guid? CausedByPointOfSaleId { get; set; }
        public Guid? CausedByUserId { get; set; }
    }
}
