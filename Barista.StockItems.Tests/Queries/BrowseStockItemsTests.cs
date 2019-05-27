using Barista.Common.Tests;
using Barista.StockItems.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Barista.StockItems.Tests.Queries
{
    [TestClass]
    public class BrowseStockItemsTests : BrowseQueryTestBase<BrowseStockItems, Domain.StockItem>
    {
        protected override BrowseStockItems InstantiateQuery() => new BrowseStockItems();

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseStockItems.DisplayName),
                QueryConfiguratorAction = q => q.DisplayName = "Abc",
                SampleData = new []
                {
                    new Domain.StockItem(TestIds.A, "Does not contain", Guid.Empty),
                    new Domain.StockItem(TestIds.B, "Does contain Abc", Guid.Empty)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowseStockItems.AtPointOfSaleId),
                QueryConfiguratorAction = q => q.AtPointOfSaleId = TestIds.D,
                SampleData = new []
                {
                    new Domain.StockItem(TestIds.A, "First", TestIds.C),
                    new Domain.StockItem(TestIds.B, "Second", TestIds.D)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            }
        };        
    }
}
