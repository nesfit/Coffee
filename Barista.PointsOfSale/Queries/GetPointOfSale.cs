using System;
using Barista.Contracts;
using Barista.PointsOfSale.Dto;

namespace Barista.PointsOfSale.Queries
{
    public class GetPointOfSale : IQuery<PointOfSaleDto>
    {
        public GetPointOfSale(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
