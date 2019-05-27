using System;
using Barista.Contracts.Commands.SaleStrategy;

namespace Barista.Swipe.Commands
{
    public class ApplySaleStrategy : IApplySaleStrategy
    {
        public ApplySaleStrategy(Guid userId, Guid saleStrategyId, decimal cost)
        {
            UserId = userId;
            SaleStrategyId = saleStrategyId;
            Cost = cost;
        }

        public Guid UserId { get; }
        public Guid SaleStrategyId { get; }
        public decimal Cost { get; }
    }
}
