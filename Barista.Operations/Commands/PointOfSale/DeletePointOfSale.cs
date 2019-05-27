using System;
using Barista.Contracts.Commands.PointOfSale;

namespace Barista.Operations.Commands.PointOfSale
{
    public class DeletePointOfSale : IDeletePointOfSale
    {
        public DeletePointOfSale(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
