using System;

namespace Barista.Accounting.Domain
{
    public class SpendingOfUsers
    {
        public Guid? SaleId { get; set; }
        public Guid? PaymentId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset Created { get; set; }
        public Guid? AccountingGroupId { get; set; }
        public Guid? PointOfSaleId { get; set; }
    }
}
