using System;

namespace Barista.Contracts.Commands.AccountingGroup
{
    public interface IDeleteAccountingGroup : ICommand
    {
        Guid Id { get; }
    }
}
