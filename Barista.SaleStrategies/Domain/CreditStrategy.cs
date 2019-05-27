using System;
using System.Threading.Tasks;
using Barista.SaleStrategies.Services;

namespace Barista.SaleStrategies.Domain
{
    public class CreditStrategy : SaleStrategy
    {
        public static readonly Guid StaticId = new Guid("CAFE0000-0055-0000-0000-00000000000C");

        public CreditStrategy() : base(StaticId, "Credit")
        {

        }

        public override async Task<bool> ApplyAsync(IAccountingService accountingService, Guid userId, decimal cost)
        {
            if (accountingService == null) throw new ArgumentNullException(nameof(accountingService));

            var userBalance = await accountingService.GetBalance(userId);
            return userBalance.Value >= cost;
        }
    }
}
