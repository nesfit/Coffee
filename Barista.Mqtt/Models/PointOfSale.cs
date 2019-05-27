using System;

namespace Barista.Mqtt.Models
{
    public class PointOfSale
    {
        public Guid Id { get; set; }
        public string[] Features { get; set; }
    }
}
