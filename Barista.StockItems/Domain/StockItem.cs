using System;
using Barista.Common;

namespace Barista.StockItems.Domain
{
    public class StockItem : Entity
    {
        public string DisplayName { get; private set; }
        public Guid PointOfSaleId { get; private set; }

        public StockItem(Guid id, string displayName, Guid pointOfSaleId) : base(id)
        {
            SetDisplayName(displayName);
            SetPointOfSaleId(pointOfSaleId);
        }

        public void SetDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new BaristaException("invalid_display_name", "Display name cannot be empty.");

            DisplayName = displayName;
            SetUpdatedNow();
        }

        public void SetPointOfSaleId(Guid pointOfSaleId)
        {
            PointOfSaleId = pointOfSaleId;
            SetUpdatedNow();
        }
    }
}
