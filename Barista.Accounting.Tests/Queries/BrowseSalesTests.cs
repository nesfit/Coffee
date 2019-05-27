using System;
using System.Linq;
using Barista.Accounting.Domain;
using Barista.Accounting.Queries;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Queries
{
    [TestClass]
    public class BrowseSalesTests : BrowseQueryTestBase<BrowseSales, Sale>
    {
        protected override BrowseSales InstantiateQuery()
            => new BrowseSales();

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseSales.MadeByPointOfSale),
                QueryConfiguratorAction = q => q.MadeByPointOfSale = TestIds.D,
                SampleData = new[]
                {
                    new Sale(TestIds.A, 5, 1, Guid.Empty, Guid.Empty, Guid.Empty, TestIds.B, Guid.Empty, Guid.Empty),
                    new Sale(TestIds.C, 5, 1, Guid.Empty, Guid.Empty, Guid.Empty, TestIds.D, Guid.Empty, Guid.Empty)
                },
                SampleDataValidator = x => Assert.AreEqual(TestIds.C, x.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowseSales.MadeByUser),
                QueryConfiguratorAction = q => q.MadeByUser = TestIds.D,
                SampleData = new[]
                {
                    new Sale(TestIds.A, 5, 1, Guid.Empty, TestIds.B, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty),
                    new Sale(TestIds.C, 5, 1, Guid.Empty, TestIds.D, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty)
                },
                SampleDataValidator = x => Assert.AreEqual(TestIds.C, x.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowseSales.MadeInAccountingGroup),
                QueryConfiguratorAction = q => q.MadeInAccountingGroup = TestIds.D,
                SampleData = new[]
                {
                    new Sale(TestIds.A, 5, 1, TestIds.B, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty),
                    new Sale(TestIds.C, 5, 1, TestIds.D, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty)
                },
                SampleDataValidator = x => Assert.AreEqual(TestIds.C, x.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowseSales.NewerThan),
                QueryConfiguratorAction = q => q.NewerThan = TestDateTimes.Year2002,
                SampleData = new[]
                {
                    new Sale(TestIds.A, 5, 1, TestIds.B, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, TestDateTimes.Year2001),
                    new Sale(TestIds.C, 5, 1, TestIds.D, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, TestDateTimes.Year2003)
                },
                SampleDataValidator = x => Assert.AreEqual(TestIds.C, x.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowseSales.OlderThan),
                QueryConfiguratorAction = q => q.OlderThan = TestDateTimes.Year2002,
                SampleData = new[]
                {
                    new Sale(TestIds.A, 5, 1, TestIds.B, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, TestDateTimes.Year2001),
                    new Sale(TestIds.C, 5, 1, TestIds.D, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, TestDateTimes.Year2003)
                },
                SampleDataValidator = x => Assert.AreEqual(TestIds.A, x.Single().Id)
            }
        };
    }
}
