using System.Linq;
using Barista.Accounting.Domain;
using Barista.Accounting.Queries;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Queries
{
    [TestClass]
    public class BrowsePaymentsTests : BrowseQueryTestBase<BrowsePayments, Payment>
    {
        protected override BrowsePayments InstantiateQuery()
            => new BrowsePayments();

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowsePayments.CreditedToUser),
                QueryConfiguratorAction = bp => bp.CreditedToUser = TestIds.D,
                SampleData = new[]
                {
                    new Payment(TestIds.A, 5, TestIds.B, "Src", "Id"),
                    new Payment(TestIds.C, 5, TestIds.D, "Src", "Id")
                },
                SampleDataValidator = x => Assert.AreEqual(TestIds.C, x.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowsePayments.NewerThan),
                QueryConfiguratorAction = bp => bp.NewerThan = TestDateTimes.Year2003,
                SampleData = new[]
                {
                    new Payment(TestIds.A, 5, TestIds.B, "Src", "Id", TestDateTimes.Year2002 ),
                    new Payment(TestIds.C, 5, TestIds.D, "Src", "Id", TestDateTimes.Year2004 )
                },
                SampleDataValidator = x => Assert.AreEqual(TestIds.C, x.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowsePayments.OlderThan),
                QueryConfiguratorAction = bp => bp.OlderThan = TestDateTimes.Year2003,
                SampleData = new[]
                {
                    new Payment(TestIds.A, 5, TestIds.B, "Src", "Id", TestDateTimes.Year2002 ),
                    new Payment(TestIds.C, 5, TestIds.D, "Src", "Id", TestDateTimes.Year2004 )
                },
                SampleDataValidator = x => Assert.AreEqual(TestIds.A, x.Single().Id)
            }
        };
    }
}
