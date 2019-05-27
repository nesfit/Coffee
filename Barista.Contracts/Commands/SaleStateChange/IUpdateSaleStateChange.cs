using System;

namespace Barista.Contracts.Commands.SaleStateChange
{
    public interface IUpdateSaleStateChange : ICommand
    {
        Guid Id { get; }
        Guid ParentSaleId { get; }
        string Reason { get; }
        Guid? CausedByPointOfSaleId { get; }
        Guid? CausedByUserId { get; }
    }
}
