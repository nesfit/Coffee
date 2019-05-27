using System;

namespace Barista.Contracts.Commands.Product
{
    public interface ICreateProduct : ICommand
    {
        Guid Id { get; }
        string DisplayName { get; }
        decimal? RecommendedPrice { get; }
    }
}
