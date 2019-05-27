using System;
using Barista.Contracts.Commands.Product;

namespace Barista.Api.Commands.Product
{
    public class DeleteProduct : IDeleteProduct
    {
        public DeleteProduct(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
