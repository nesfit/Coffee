using System;

namespace Barista.Contracts.Events.AccountingGroup
{
    public interface IAccountingGroupDeleted : IEvent
    {
        Guid Id { get; }
    }
}
