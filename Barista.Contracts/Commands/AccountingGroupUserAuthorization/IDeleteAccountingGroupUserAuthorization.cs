using System;

namespace Barista.Contracts.Commands.AccountingGroupUserAuthorization
{
    public interface IDeleteAccountingGroupUserAuthorization : ICommand
    {
        Guid AccountingGroupId { get; }
        Guid UserId { get; }
    }
}
