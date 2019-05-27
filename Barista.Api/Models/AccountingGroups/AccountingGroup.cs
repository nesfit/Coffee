using System;

namespace Barista.Api.Models.AccountingGroups
{
    public class AccountingGroup
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Guid SaleStrategyId { get; set; }
    }
}
