using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common.Tests;
using Barista.SaleStrategies.Domain;
using Barista.SaleStrategies.Queries;
using Barista.SaleStrategies.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.SaleStrategies.Tests.Queries
{
    [TestClass]
    public class BrowseSaleStrategiesTests : BrowseQueryTestBase<BrowseSaleStrategies, Domain.SaleStrategy>
    {
        protected override BrowseSaleStrategies InstantiateQuery()
            => new BrowseSaleStrategies();

        private class TestStrategy : SaleStrategy
        {
            public TestStrategy(Guid id, string displayName) : base(id, displayName)
            {

            }

            public override Task<bool> ApplyAsync(IAccountingService accountingService, Guid userId, decimal cost)
            {
                throw new NotImplementedException();
            }
        }

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseSaleStrategies.DisplayName),
                QueryConfiguratorAction = q => q.DisplayName = "Abc",
                SampleData = new []
                {
                    new TestStrategy(TestIds.A, "Does not contain"),
                    new TestStrategy(TestIds.B, "Does contain Abc"), 
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            }
        };
    }
}
