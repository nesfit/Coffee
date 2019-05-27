using System;

namespace Barista.Contracts.Commands.AccountingGroupUserAuthorization
{
    public interface ICreateAccountingGroupUserAuthorization : ICommand
    {
        Guid AccountingGroupId { get; }
        Guid UserId { get; }
        string Level { get; }
    }
}
