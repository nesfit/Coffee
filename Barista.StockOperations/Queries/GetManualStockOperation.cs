using System;
using Barista.Contracts;
using Barista.StockOperations.Dto;

namespace Barista.StockOperations.Queries
{
    public class GetManualStockOperation : IQuery<ManualStockOperationDto>
    {
        public GetManualStockOperation(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
