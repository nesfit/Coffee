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
    public class BrowseManualStockOperationsTests : BrowseQueryTestBase<BrowseManualStockOperations, Domain.ManualStockOperation>
    {
        protected override BrowseManualStockOperations InstantiateQuery() => new BrowseManualStockOperations();

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseManualStockOperations.StockItemId),
                QueryConfiguratorAction = q => q.StockItemId = new [] { TestIds.C, TestIds.E },
                SampleData = new []
                {
                    new ManualStockOperation(TestIds.A, Guid.Empty, 1, Guid.Empty, string.Empty),
                    new ManualStockOperation(TestIds.B, TestIds.C, 1, Guid.Empty, string.Empty),
                    new ManualStockOperation(TestIds.C, TestIds.C, 1, Guid.Empty, string.Empty),
                    new ManualStockOperation(TestIds.D, TestIds.E, 1, Guid.Empty, string.Empty),
                },
                SampleDataValidator = data => Assert.IsTrue(data.Select(op => op.Id).SequenceEqual(new [] { TestIds.B, TestIds.C, TestIds.D }))
            },

            new QueryOption
            {
                Name = nameof(BrowseManualStockOperations.ByUserId),
                QueryConfiguratorAction = q => q.ByUserId = TestIds.E,
                SampleData = new []
                {
                    new ManualStockOperation(TestIds.A, Guid.Empty, 1, Guid.Empty, string.Empty),
                    new ManualStockOperation(TestIds.B, Guid.Empty, 1, TestIds.E, string.Empty),
                    new ManualStockOperation(TestIds.C, Guid.Empty, 1, TestIds.E, string.Empty),
                    new ManualStockOperation(TestIds.D, Guid.Empty, 1, Guid.Empty, string.Empty),
                },
                SampleDataValidator = data => Assert.IsTrue(data.Select(op => op.Id).SequenceEqual(new [] { TestIds.B, TestIds.C }))
            }
        };
    }
}
