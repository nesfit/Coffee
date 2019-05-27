using System;
using System.Linq;
using Barista.Common;
using Barista.Common.Tests;
using Barista.Identity.Domain;
using Barista.Identity.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Queries
{
    [TestClass]
    public class BrowseValidUserPasswordsInternalTests : BrowseQueryTestBase<BrowseValidPasswordsInternal, AuthenticationMeans>
    {
        protected override BrowseValidPasswordsInternal InstantiateQuery()
            => new BrowseValidPasswordsInternal(new PagedQuery(), new [] {TestIds.B, TestIds.D});

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = "TheOnlyOne_QueryIsNotCustomizable",
                QueryConfiguratorAction = q => { },
                SampleData = new []
                {
                    // Started in the past, ended in the past - INVALID
                    new AuthenticationMeans(TestIds.A, "password", "val", TestDateTimes.Year2001, TestDateTimes.Year2002), 

                    // Started in the future - INVALID
                    new AuthenticationMeans(TestIds.A, "password", "val", DateTimeOffset.MaxValue, null),

                    // Started in the past, not ended - VALID, but type is not password
                    new AuthenticationMeans(TestIds.B, "password2", "val", TestDateTimes.Year2001, null),
                    
                    // Started in the past, ended in the future - VALID, but means ID is not in allowed array
                    new AuthenticationMeans(TestIds.C, "password", "val", TestDateTimes.Year2001, DateTimeOffset.MaxValue),

                    // Started in the past, ended in the future - VALID, and means ID is in allowed array
                    new AuthenticationMeans(TestIds.D, "password", "val", TestDateTimes.Year2002, DateTimeOffset.MaxValue)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.D, data.Single().Id)
            }
        };
    }
}
