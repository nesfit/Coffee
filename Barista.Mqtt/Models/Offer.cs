using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barista.Mqtt.Models
{
    public class Offer
    {
        public Guid Id { get; set; }
        public Guid PointOfSaleId { get; set; }
        public Guid ProductId { get; set; }
    }
}
