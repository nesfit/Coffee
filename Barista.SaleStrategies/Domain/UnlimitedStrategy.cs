using System;
using System.Threading.Tasks;
using Barista.SaleStrategies.Services;

namespace Barista.SaleStrategies.Domain
{
    public class UnlimitedStrategy : SaleStrategy
    {
        public static readonly Guid StaticId = new Guid("CAFE0000-0055-0000-0000-00000000000A");

        public UnlimitedStrategy() : base(StaticId, "Unlimited")
        {

        }

        public override Task<bool> ApplyAsync(IAccountingService accountingService, Guid userId, decimal cost)
        {
            return Task.FromResult(true);
        }
    }
}
