using System;

namespace Barista.Contracts.Events.Product
{
    public interface IProductCreated : IEvent
    {
        Guid Id { get; }
        string DisplayName { get; }
        decimal? RecommendedPrice { get; }
    }
}
