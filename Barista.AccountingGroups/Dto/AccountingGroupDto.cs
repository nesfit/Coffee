using System;

namespace Barista.AccountingGroups.Dto
{
    public class AccountingGroupDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Guid SaleStrategyId { get; set; }
    }
}
