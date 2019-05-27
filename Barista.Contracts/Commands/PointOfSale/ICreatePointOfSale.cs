using System;

namespace Barista.Contracts.Commands.PointOfSale
{
    public interface ICreatePointOfSale : ICommand
    {
        Guid Id { get; }
        string DisplayName { get; }
        Guid ParentAccountingGroupId { get; }
        Guid? SaleStrategyId { get; }
        string[] Features { get; }
    }
}
