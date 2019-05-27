using System;

namespace Barista.Swipe.Models
{
    public class AccountingGroup
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Guid SaleStrategyId { get; set; }
    }
}
