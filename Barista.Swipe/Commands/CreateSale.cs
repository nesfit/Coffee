using System;
using Barista.Contracts.Commands.Sale;

namespace Barista.Swipe.Commands
{
    public class CreateSale : ICreateSale
    {
        public CreateSale(Guid id, decimal cost, decimal quantity, Guid accountingGroupId, Guid userId, Guid authenticationMeansId, Guid pointOfSaleId, Guid productId, Guid offerId)
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
