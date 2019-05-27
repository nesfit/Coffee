using System;
using System.Linq;
using Barista.Common.Tests;
using Barista.Offers.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Offers.Tests.Queries
{
    [TestClass]
    public class BrowseOffersTests : BrowseQueryTestBase<BrowseOffers, Domain.Offer>
    {
        protected override BrowseOffers InstantiateQuery()
            => new BrowseOffers();

        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseOffers.ValidAt),
                QueryConfiguratorAction = q => q.ValidAt = TestDateTimes.Year2003,
                SampleData = new []
                {
                    // Started in the past, ended in the past
                    new Domain.Offer(TestIds.A, Guid.Empty, Guid.Empty, null, null, TestDateTimes.Year2001, TestDateTimes.Year2002), 

                    // Started in the past, not ended
                    new Domain.Offer(TestIds.B, Guid.Empty, Guid.Empty, null, null, TestDateTimes.Year2001, null),
                    
                    // Started in the past, ended in the future
                    new Domain.Offer(TestIds.C, Guid.Empty, Guid.Empty, null, null, TestDateTimes.Year2001, DateTimeOffset.MaxValue),

                    // Started in the future
                    new Domain.Offer(TestIds.D, Guid.Empty, Guid.Empty, null, null, DateTimeOffset.MaxValue, null),
                },
                SampleDataValidator = data => Assert.IsTrue(data.Select(a => a.Id).SequenceEqual(new [] { TestIds.B, TestIds.C }))
            },

            new QueryOption
            {
                Name = nameof(BrowseOffers.AtPointOfSaleId),
                QueryConfiguratorAction = q => q.AtPointOfSaleId = TestIds.C,
                SampleData = new []
                {
                    new Domain.Offer(TestIds.A, Guid.Empty, Guid.Empty, null, null, null, null),
                    new Domain.Offer(TestIds.B, TestIds.C, Guid.Empty, null, null, null, null),
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowseOffers.OfProductId),
                QueryConfiguratorAction = q => q.OfProductId = TestIds.C,
                SampleData = new []
                {
                    new Domain.Offer(TestIds.A, Guid.Empty, Guid.Empty, null, null, null, null),
                    new Domain.Offer(TestIds.B, Guid.Empty, TestIds.C, null, null, null, null),
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().Id)
            },

            new QueryOption
            {
                Name = nameof(BrowseOffers.OfStockItemId),
                QueryConfiguratorAction = q => q.OfStockItemId = TestIds.D,
                SampleData = new []
                {
                    new Domain.Offer(TestIds.A, Guid.Empty, Guid.Empty, null, null, null, null),
                    new Domain.Offer(TestIds.B, Guid.Empty, Guid.Empty, null, Guid.Empty, null, null),
                    new Domain.Offer(TestIds.C, Guid.Empty, Guid.Empty, null, TestIds.D, null, null),
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.C, data.Single().Id)
            }
        };

    }
}
