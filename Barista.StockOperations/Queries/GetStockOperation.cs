using System;
using Barista.Contracts;
using Barista.StockOperations.Dto;

namespace Barista.StockOperations.Queries
{
    public class GetStockOperation : IQuery<StockOperationDto>
    {
        public GetStockOperation(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
