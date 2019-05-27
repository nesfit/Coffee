using System.Linq;
using Barista.Common.Tests;
using Barista.Users.Domain;
using Barista.Users.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Users.Tests.Queries
{
    [TestClass]
    public class BrowseUsersTests : BrowseQueryTestBase<BrowseUsers, User>
    {
        protected override BrowseUsers InstantiateQuery() => new BrowseUsers();

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseUsers.EmailAddress),
                QueryConfiguratorAction = q => q.EmailAddress = "domain.tld",
                SampleData = new []
                {
                    new User(TestIds.A, "Someone", "not_this@example.com", false),
                    new User(TestIds.B, "Someone", "thats_me@domain.tld", false),
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowseUsers.FullName),
                QueryConfiguratorAction = q => q.FullName = "Smith",
                SampleData = new []
                {
                    new User(TestIds.A, "John Doe", "not_this@example.com", false),
                    new User(TestIds.B, "James Smith", "thats_me@domain.tld", false),
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            }
        };
    }
}
