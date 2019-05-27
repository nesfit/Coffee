using System;

namespace Barista.Contracts.Events.Product
{
    public interface IProductUpdated : IEvent
    {
        Guid Id { get; }
        string DisplayName { get; }
        decimal? RecommendedPrice { get; }
    }
}
