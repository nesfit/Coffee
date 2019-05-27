using System;
using System.Collections.Generic;
using System.Text;
using Barista.StockOperations.Domain;

namespace Barista.StockOperations.Tests
{
    internal class TestStockOperation : StockOperation
    {
        public TestStockOperation(Guid id, Guid stockItemId, decimal quantity) : base(id, stockItemId, quantity)
        {
        }
    }
}
