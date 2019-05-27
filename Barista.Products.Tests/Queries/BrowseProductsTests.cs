using Barista.Common.Tests;
using Barista.Products.Domain;
using Barista.Products.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Barista.Products.Tests.Queries
{
    public class BrowseProductsTests : BrowseQueryTestBase<BrowseProducts, Product>
    {
        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseProducts.DisplayName),
                QueryConfiguratorAction = q => q.DisplayName = "Abc",
                SampleData = new []
                {
                    new Product(TestIds.A, "Does not contain", null),
                    new Product(TestIds.B, "Does contain Abc", null)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            }
        };

        protected override BrowseProducts InstantiateQuery() => new BrowseProducts();
    }
}
