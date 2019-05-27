using Barista.AccountingGroups.Domain;
using Barista.AccountingGroups.Queries;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Barista.AccountingGroups.Tests.Queries
{
    [TestClass]
    public class BrowseAccountingGroupsQueryTests : BrowseQueryTestBase<BrowseAccountingGroups, AccountingGroup>
    {
        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseAccountingGroups.DisplayName),
                QueryConfiguratorAction = q => q.DisplayName = "Abc",
                SampleData = new []
                {
                    new AccountingGroup(TestIds.A, "Does not contain", Guid.Empty),
                    new AccountingGroup(TestIds.B, "Does contain Abc", Guid.Empty)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            }
        };

        protected override BrowseAccountingGroups InstantiateQuery() => new BrowseAccountingGroups();
    }
}
