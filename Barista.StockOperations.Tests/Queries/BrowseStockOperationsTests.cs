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
    public class BrowseStockOperationsTests : BrowseQueryTestBase<BrowseStockOperations, Domain.StockOperation>
    {
        protected override BrowseStockOperations InstantiateQuery() => new BrowseStockOperations();

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseStockOperations.StockItemId),
                QueryConfiguratorAction = q => q.StockItemId = new [] { TestIds.C, TestIds.E },
                SampleData = new []
                {
                    new TestStockOperation(TestIds.A, Guid.Empty, 1),
                    new TestStockOperation(TestIds.B, TestIds.C, 1),
                    new TestStockOperation(TestIds.C, TestIds.C, 1),
                    new TestStockOperation(TestIds.D, TestIds.E, 1),
                },
                SampleDataValidator = data => Assert.IsTrue(data.Select(op => op.Id).SequenceEqual(new [] { TestIds.B, TestIds.C, TestIds.D }))
            }
        };
    }
}
