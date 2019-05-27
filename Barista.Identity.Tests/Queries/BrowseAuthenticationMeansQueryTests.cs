using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Barista.Common.Tests;
using Barista.Identity.Domain;
using Barista.Identity.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Queries
{
    [TestClass]
    public class BrowseAuthenticationMeansQueryTests : BrowseQueryTestBase<BrowseAuthenticationMeans, Domain.AuthenticationMeans>
    {
        protected override BrowseAuthenticationMeans InstantiateQuery()
            => new BrowseAuthenticationMeans();

        private const string TestValue = "Value123";
        private const string TestMethod = "Method123";

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseAuthenticationMeans.Value),
                QueryConfiguratorAction = q => q.Value = TestValue,
                SampleData = new []
                {
                    new AuthenticationMeans(TestIds.A, "Type", "Value", TestDateTimes.Year2001, null),
                    new AuthenticationMeans(TestIds.B, "Type", TestValue, TestDateTimes.Year2001, null),
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowseAuthenticationMeans.Method),
                QueryConfiguratorAction = q => q.Method = TestMethod,
                SampleData = new []
                {
                    new AuthenticationMeans(TestIds.A, "Method", "Value", TestDateTimes.Year2001, null),
                    new AuthenticationMeans(TestIds.B, TestMethod, "Value", TestDateTimes.Year2001, null),
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            }
        };
    }
}
