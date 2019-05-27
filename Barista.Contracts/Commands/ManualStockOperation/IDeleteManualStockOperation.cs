using System;

namespace Barista.Contracts.Commands.ManualStockOperation
{
    public interface IDeleteManualStockOperation : ICommand
    {
        Guid Id { get; }
    }
}
