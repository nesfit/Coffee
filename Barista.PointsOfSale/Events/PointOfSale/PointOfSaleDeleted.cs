using System;
using Barista.Contracts.Events.PointOfSale;

namespace Barista.PointsOfSale.Events.PointOfSale
{
    public class PointOfSaleDeleted : IPointOfSaleDeleted
    { 
        public PointOfSaleDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
