using System;
using System.Linq;
using Barista.Common.Tests;
using Barista.Identity.Domain;
using Barista.Identity.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Queries
{
    [TestClass]
    public class BrowseAssignmentsQueryTests : BrowseQueryTestBase<BrowseAssignments, Domain.Assignment>
    {
        protected override BrowseAssignments InstantiateQuery()
            => new BrowseAssignments();

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseAssignments.MustBeValid),
                QueryConfiguratorAction = q => q.MustBeValid = true,
                SampleData = new []
                {
                    // Started in the past, ended in the past
                    new AssignmentToPointOfSale(TestIds.A, Guid.Empty, TestDateTimes.Year2001, TestDateTimes.Year2002, Guid.Empty), 

                    // Started in the past, not ended
                    new AssignmentToPointOfSale(TestIds.B, Guid.Empty, TestDateTimes.Year2001, null, Guid.Empty),
                    
                    // Started in the past, ended in the future
                    new AssignmentToPointOfSale(TestIds.C, Guid.Empty, TestDateTimes.Year2001, DateTimeOffset.MaxValue, Guid.Empty),

                    // Started in the future
                    new AssignmentToPointOfSale(TestIds.D, Guid.Empty, DateTimeOffset.MaxValue, null, Guid.Empty),
                },
                SampleDataValidator = data => Assert.IsTrue(data.Select(a => a.Id).SequenceEqual(new [] { TestIds.B, TestIds.C }))
            },

            new QueryOption
            {
                Name = nameof(BrowseAssignments.OfAuthenticationMeans),
                QueryConfiguratorAction = q => q.OfAuthenticationMeans = TestIds.A,
                SampleData = new []
                {
                    // Started in the past, ended in the past
                    new AssignmentToPointOfSale(TestIds.A, Guid.Empty, TestDateTimes.Year2001, null, Guid.Empty), 

                    // Started in the past, not ended
                    new AssignmentToPointOfSale(TestIds.B, TestIds.A, TestDateTimes.Year2001, null, Guid.Empty)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            }
        };

    }
}
