using System;
using Barista.Contracts.Events.Product;

namespace Barista.Products.Events
{
    public class ProductDeleted : IProductDeleted
    {
        public ProductDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
