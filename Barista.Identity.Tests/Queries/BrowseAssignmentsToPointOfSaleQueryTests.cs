using System;
using System.Linq;
using Barista.Common.Tests;
using Barista.Identity.Domain;
using Barista.Identity.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Queries
{
    [TestClass]
    public class BrowseAssignmentsToPointOfSaleQueryTests : BrowseQueryTestBase<BrowseAssignmentsToPointOfSale, Domain.AssignmentToPointOfSale>
    {
        protected override BrowseAssignmentsToPointOfSale InstantiateQuery()
            => new BrowseAssignmentsToPointOfSale();

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseAssignmentsToPointOfSale.MustBeValid),
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
                Name = nameof(BrowseAssignmentsToPointOfSale.OfAuthenticationMeans),
                QueryConfiguratorAction = q => q.OfAuthenticationMeans = TestIds.A,
                SampleData = new []
                {
                    // Started in the past, ended in the past
                    new AssignmentToPointOfSale(TestIds.A, Guid.Empty, TestDateTimes.Year2001, null, Guid.Empty), 

                    // Started in the past, not ended
                    new AssignmentToPointOfSale(TestIds.B, TestIds.A, TestDateTimes.Year2001, null, Guid.Empty)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowseAssignmentsToPointOfSale.AssignedToPointOfSale),
                QueryConfiguratorAction = q => q.AssignedToPointOfSale = TestIds.C,
                SampleData = new []
                {
                    // Started in the past, ended in the past
                    new AssignmentToPointOfSale(TestIds.A, Guid.Empty, TestDateTimes.Year2001, null, Guid.Empty), 

                    // Started in the past, not ended
                    new AssignmentToPointOfSale(TestIds.B, Guid.Empty, TestDateTimes.Year2001, null, TestIds.C)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            }
        };

    }
}
