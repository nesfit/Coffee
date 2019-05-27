using System;
using System.Linq;
using Barista.Common.Tests;
using Barista.PointsOfSale.Domain;
using Barista.PointsOfSale.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.PointsOfSale.Tests.Queries
{
    [TestClass]
    public class BrowsePointsOfSaleTests : BrowseQueryTestBase<BrowsePointsOfSale, PointOfSale>
    {
        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowsePointsOfSale.DisplayName),
                QueryConfiguratorAction = q => q.DisplayName = "Abc",
                SampleData = new []
                {
                    new PointOfSale(TestIds.A, "Does not contain", Guid.Empty, null),
                    new PointOfSale(TestIds.B, "Does contain Abc", Guid.Empty, null)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            }
        };

        protected override BrowsePointsOfSale InstantiateQuery() => new BrowsePointsOfSale();
    }
}
