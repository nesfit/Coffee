using System;
using Barista.Contracts;
using Barista.Products.Dto;

namespace Barista.Products.Queries
{
    public class GetProduct : IQuery<ProductDto>
    {
        public GetProduct(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
