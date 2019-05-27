using System;
using Barista.Contracts.Commands.PointOfSale;

namespace Barista.Mqtt.Commands
{
    public class SetPointOfSaleKeyValue : ISetPointOfSaleKeyValue
    {
        public SetPointOfSaleKeyValue(Guid pointOfSaleId, string key, string value)
        {
            PointOfSaleId = pointOfSaleId;
            Key = key;
            Value = value;
        }

        public Guid PointOfSaleId { get; }
        public string Key { get; }
        public string Value { get; }
    }
}
