using System;
using Barista.Contracts.Events.PointOfSale;

namespace Barista.PointsOfSale.Events.PointOfSale
{
    public class PointOfSaleKeyValueUpdated : IPointOfSaleKeyValueUpdated
    {
        public PointOfSaleKeyValueUpdated(Guid pointOfSaleId, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            PointOfSaleId = pointOfSaleId;
            Key = key;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public Guid PointOfSaleId { get; }
        public string Key { get; }
        public string Value { get; }
    }
}
