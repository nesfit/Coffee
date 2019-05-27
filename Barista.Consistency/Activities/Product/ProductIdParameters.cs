using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barista.Consistency.Activities.Product
{
    public class ProductIdParameters : ConsistencyActivityParametersBase
    {
        public Guid ProductId { get; set; }
    }
}
