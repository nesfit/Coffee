using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Contracts;
using Barista.SaleStrategies.Dto;

namespace Barista.SaleStrategies.Queries
{
    public class GetSaleStrategy : IQuery<SaleStrategyDto>
    {
        public GetSaleStrategy(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
