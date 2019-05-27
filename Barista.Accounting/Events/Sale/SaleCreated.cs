using System;
using Barista.Contracts.Events.Sale;

namespace Barista.Accounting.Events.Sale
{
    public class SaleCreated : ISaleCreated
    {
        public SaleCreated(Guid id, decimal cost, decimal quantity, Guid accountingGroupId, Guid userId, Guid authenticationMeansId, Guid pointOfSaleId, Guid productId, Guid offerId)
        {
            Id = id;
            Cost = cost;
            Quantity = quantity;
            AccountingGroupId = accountingGroupId;
            UserId = userId;
            AuthenticationMeansId = authenticationMeansId;
            PointOfSaleId = pointOfSaleId;
            ProductId = productId;
            OfferId = offerId;
        }

        public SaleCreated(Domain.Sale sale) : this(sale.Id, sale.Cost, sale.Quantity, sale.AccountingGroupId, sale.UserId, sale.AuthenticationMeansId, sale.PointOfSaleId, sale.ProductId, sale.OfferId)
        {

        }

        public Guid Id { get; }
        public decimal Cost { get; }
        public decimal Quantity { get; }
        public Guid AccountingGroupId { get; }
        public Guid UserId { get; }
        public Guid AuthenticationMeansId { get; }
        public Guid PointOfSaleId { get; }
        public Guid ProductId { get; }
        public Guid OfferId { get; }
    }
}
