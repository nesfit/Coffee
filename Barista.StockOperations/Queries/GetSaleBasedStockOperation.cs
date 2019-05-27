using System;
using Barista.Contracts;
using Barista.StockOperations.Dto;

namespace Barista.StockOperations.Queries
{
    public class GetSaleBasedStockOperation : IQuery<SaleBasedStockOperationDto>
    {
        public GetSaleBasedStockOperation(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
