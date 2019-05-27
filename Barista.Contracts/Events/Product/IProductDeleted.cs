using System;

namespace Barista.Contracts.Events.Product
{
    public interface IProductDeleted : IEvent
    {
        Guid Id { get; }
    }
}
