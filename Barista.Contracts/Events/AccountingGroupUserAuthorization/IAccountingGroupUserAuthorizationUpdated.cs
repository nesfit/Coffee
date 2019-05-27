using System;

namespace Barista.Contracts.Events.AccountingGroupUserAuthorization
{
    public interface IAccountingGroupUserAuthorizationUpdated : IEvent
    {
        Guid AccountingGroupId { get; }
        Guid UserId { get; }
        string Level { get; }
    }
}
