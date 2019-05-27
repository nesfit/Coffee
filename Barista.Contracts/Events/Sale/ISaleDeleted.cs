using System;

namespace Barista.Contracts.Events.Sale
{
    public interface ISaleDeleted : IEvent
    {
        Guid Id { get; }
    }
}
