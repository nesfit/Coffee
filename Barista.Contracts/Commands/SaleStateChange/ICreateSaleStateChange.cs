using System;

namespace Barista.Contracts.Commands.SaleStateChange
{
    public interface ICreateSaleStateChange : ICommand
    {
        Guid ParentSaleId { get; }
        Guid Id { get; }
        string Reason { get; }
        string State { get; }
        Guid? CausedByPointOfSaleId { get; }
        Guid? CausedByUserId { get; }
    }
}
