using System;

namespace Barista.Contracts.Commands.Operations
{
    public interface IHandleCreationOfAccountingGroup : AccountingGroup.ICreateAccountingGroup, ICommand
    {
        Guid OwnerUserId { get; }
    }
}
