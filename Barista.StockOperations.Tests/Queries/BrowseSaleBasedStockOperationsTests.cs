using Barista.Common.Tests;
using Barista.StockOperations.Domain;
using Barista.StockOperations.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Barista.StockOperations.Tests.Queries
{
    [TestClass]
    public class BrowseSaleBasedStockOperationsTests : BrowseQueryTestBase<BrowseSaleBasedStockOperations, Domain.SaleBasedStockOperation>
    {
        protected override BrowseSaleBasedStockOperations InstantiateQuery() => new BrowseSaleBasedStockOperations();

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseSaleBasedStockOperations.StockItemId),
                QueryConfiguratorAction = q => q.StockItemId = new [] { TestIds.C, TestIds.E },
                SampleData = new []
                {
                    new SaleBasedStockOperation(TestIds.A, Guid.Empty, 1, Guid.Empty),
                    new SaleBasedStockOperation(TestIds.B, TestIds.C, 1, Guid.Empty),
                    new SaleBasedStockOperation(TestIds.C, TestIds.C, 1, Guid.Empty),
                    new SaleBasedStockOperation(TestIds.D, TestIds.E, 1, Guid.Empty),
                },
                SampleDataValidator = data => Assert.IsTrue(data.Select(op => op.Id).SequenceEqual(new [] { TestIds.B, TestIds.C, TestIds.D }))
            },

            new QueryOption
            {
                Name = nameof(BrowseSaleBasedStockOperations.SaleId),
                QueryConfiguratorAction = q => q.SaleId = TestIds.E,
                SampleData = new []
                {
                    new SaleBasedStockOperation(TestIds.A, Guid.Empty, 1, Guid.Empty),
                    new SaleBasedStockOperation(TestIds.B, Guid.Empty, 1, TestIds.E),
                    new SaleBasedStockOperation(TestIds.C, Guid.Empty, 1, TestIds.E),
                    new SaleBasedStockOperation(TestIds.D, Guid.Empty, 1, Guid.Empty),
                },
                SampleDataValidator = data => Assert.IsTrue(data.Select(op => op.Id).SequenceEqual(new [] { TestIds.B, TestIds.C }))
            }
        };
    }
}
