using System;
using Barista.Common;

namespace Barista.PointsOfSale.Domain
{
    public class PointOfSaleKeyValue
    {
        public Guid Id { get; protected set; }
        public string Key { get; protected set; }
        public string Value { get; protected set; }
        public DateTimeOffset LastUpdated { get; protected set; }

        public PointOfSaleKeyValue(string key, string value) : this(Guid.NewGuid(), key, value, DateTimeOffset.UtcNow)
        {
        }

        protected PointOfSaleKeyValue(Guid id, string key, string value, DateTimeOffset lastUpdated)
        {
            Id = id;
            SetKey(key);
            SetValue(value);
            SetLastUpdated(lastUpdated);
        }
        
        public void SetValue(string value)
        {
            Value = value ?? throw new BaristaException("invalid_value", "Value cannot be null");
            SetLastUpdated(DateTimeOffset.UtcNow);
        }

        protected void SetKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new BaristaException("invalid_key", "Key cannot be null or whitespace.");

            Key = key;
        }

        protected void SetLastUpdated(DateTimeOffset lastUpdated)
        {
            LastUpdated = lastUpdated;
        }
    }
}
