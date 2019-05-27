using System;

namespace Barista.Contracts.Events.AccountingGroupUserAuthorization
{
    public interface IAccountingGroupUserAuthorizationDeleted : IEvent
    {
        Guid AccountingGroupId { get; }
        Guid UserId { get; }
    }
}
