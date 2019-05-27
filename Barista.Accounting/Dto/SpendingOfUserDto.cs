using System;

namespace Barista.Accounting.Dto
{
    public class SpendingOfUserDto
    {
        public Guid? SaleId { get; set; }
        public Guid? PaymentId { get; set; }
        public Guid? AccountingGroupId { get; set; }
        public Guid? PointOfSaleId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
