using System;

namespace Barista.Contracts.Commands.SaleStrategy
{
    public interface IApplySaleStrategy : ICommand
    {
        Guid UserId { get; }
        Guid SaleStrategyId { get; }
        decimal Cost { get; }
    }
}
