using System;
using System.Collections.Generic;
using System.Linq;
using Barista.Common;

namespace Barista.PointsOfSale.Domain
{
    public class PointOfSale : Entity
    {
        public string DisplayName { get; private set; }
        public Guid ParentAccountingGroupId { get; private set; }
        public Guid? SaleStrategyId { get; private set; }
        public IList<PointOfSaleKeyValue> KeyValues { get; private set; } = new List<PointOfSaleKeyValue>();
        public IEnumerable<string> Features { get; private set; } = new string[0];

        public PointOfSale(Guid id, string displayName, Guid parentAccountingGroupId, Guid? saleStrategyId, IEnumerable<string> features) : base(id)
        {
            SetDisplayName(displayName);
            SetParentAccountingGroupId(parentAccountingGroupId);
            SetSaleStrategyId(saleStrategyId);
            SetFeatures(features ?? new string[0]);
        }

        public void SetKeyValue(string key, string value)
        {
            bool IsMatch(PointOfSaleKeyValue posKv) => string.Equals(posKv.Key, key, StringComparison.OrdinalIgnoreCase);
            var cnt = KeyValues.Count(IsMatch);

            if (cnt > 1)
                foreach (var kv in KeyValues.Where(IsMatch).ToArray())
                    KeyValues.Remove(kv);
            else if (cnt == 1)
                KeyValues.First(IsMatch).SetValue(value);
            else
                KeyValues.Add(new PointOfSaleKeyValue(key, value));
        }

        public void SetDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new BaristaException("invalid_display_name", "Display name cannot be empty");

            DisplayName = displayName;
            SetUpdatedNow();
        }

        public void SetParentAccountingGroupId(Guid parentAccountingGroupId)
        {
            ParentAccountingGroupId = parentAccountingGroupId;
            SetUpdatedNow();
        }

        public void SetSaleStrategyId(Guid? saleStrategyId)
        {
            SaleStrategyId = saleStrategyId;
            SetUpdatedNow();
        }

        public void SetFeatures(IEnumerable<string> features)
        {
            if (features == null)
                throw new BaristaException("invalid_features", "Features cannot be null");

            Features = features.ToList();
            SetUpdatedNow();
        }
    }
}
