using System;
using Newtonsoft.Json;

namespace Barista.Operations.Activities.AccountingGroup.Create
{
    public class CreationActivityParameters
    {
        [JsonConstructor]
        public CreationActivityParameters(Guid id, string displayName, Guid saleStrategyId)
        {
            Id = id;
            DisplayName = displayName;
            SaleStrategyId = saleStrategyId;
        }

        public Guid Id { get; }
        public string DisplayName { get; }
        public Guid SaleStrategyId { get; }
    }
}