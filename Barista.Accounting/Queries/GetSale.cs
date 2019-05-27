using System;
using Barista.Accounting.Dto;
using Barista.Common;
using Barista.Contracts;

namespace Barista.Accounting.Queries
{
    public class GetSale : IQuery<SaleDto>
    {
        public Guid Id { get; }

        public GetSale(Guid id)
        {
            Id = id;
        }
    }
}
