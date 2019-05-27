using System;
using System.Linq;
using Barista.Common.Tests;
using Barista.Identity.Domain;
using Barista.Identity.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Queries
{
    [TestClass]
    public class BrowseAssignmentsToUserQueryTests : BrowseQueryTestBase<BrowseAssignmentsToUser, Domain.AssignmentToUser>
    {
        protected override BrowseAssignmentsToUser InstantiateQuery()
            => new BrowseAssignmentsToUser();

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseAssignmentsToUser.MustBeValid),
                QueryConfiguratorAction = q => q.MustBeValid = true,
                SampleData = new []
                {
                    // Started in the past, ended in the past
                    new AssignmentToUser(TestIds.A, Guid.Empty, TestDateTimes.Year2001, TestDateTimes.Year2002, Guid.Empty, false), 

                    // Started in the past, not ended
                    new AssignmentToUser(TestIds.B, Guid.Empty, TestDateTimes.Year2001, null, Guid.Empty, false),
                    
                    // Started in the past, ended in the future
                    new AssignmentToUser(TestIds.C, Guid.Empty, TestDateTimes.Year2001, DateTimeOffset.MaxValue, Guid.Empty, false),

                    // Started in the future
                    new AssignmentToUser(TestIds.D, Guid.Empty, DateTimeOffset.MaxValue, null, Guid.Empty, false),
                },
                SampleDataValidator = data => Assert.IsTrue(data.Select(a => a.Id).SequenceEqual(new [] { TestIds.B, TestIds.C }))
            },

            new QueryOption
            {
                Name = nameof(BrowseAssignmentsToUser.OfAuthenticationMeans),
                QueryConfiguratorAction = q => q.OfAuthenticationMeans = TestIds.A,
                SampleData = new []
                {
                    // Started in the past, ended in the past
                    new AssignmentToUser(TestIds.A, Guid.Empty, TestDateTimes.Year2001, null, Guid.Empty, false), 

                    // Started in the past, not ended
                    new AssignmentToUser(TestIds.B, TestIds.A, TestDateTimes.Year2001, null, Guid.Empty, false)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowseAssignmentsToUser.AssignedToUser),
                QueryConfiguratorAction = q => q.AssignedToUser = TestIds.C,
                SampleData = new []
                {
                    // Started in the past, ended in the past
                    new AssignmentToUser(TestIds.A, Guid.Empty, TestDateTimes.Year2001, null, Guid.Empty, false), 

                    // Started in the past, not ended
                    new AssignmentToUser(TestIds.B, Guid.Empty, TestDateTimes.Year2001, null, TestIds.C, false)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            }
        };

    }
}
