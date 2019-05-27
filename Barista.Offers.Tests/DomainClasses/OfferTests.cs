using System;
using Barista.Common;
using Barista.Common.Tests;
using Barista.Offers.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Offers.Tests.DomainClasses
{
    [TestClass]
    public class OfferTests
    {
        [TestMethod]
        public void SetValidity_RejectsInvalidValue()
        {
            var ex = Assert.ThrowsException<BaristaException>(() => new Offer(Guid.Empty, Guid.Empty, Guid.Empty, null, null, TestDateTimes.Year2005, TestDateTimes.Year2001));
            Assert.AreEqual("invalid_validity", ex.Code);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow(2001, null)]
        [DataRow(null, 2001)]
        [DataRow(2001, 2002)]
        public void SetValidity_AcceptsValidValues(int? startOfValidityYear, int? endOfValidityYear)
        {
            DateTimeOffset? startOfValidity = null;
            DateTimeOffset? endOfValidity = null;

            if (startOfValidityYear != null)
                startOfValidity = new DateTimeOffset(startOfValidityYear.Value, 1 , 1, 1, 1, 1, TimeSpan.Zero);

            if (endOfValidityYear != null)
                endOfValidity = new DateTimeOffset(endOfValidityYear.Value, 1, 1, 1, 1, 1, TimeSpan.Zero);

            Assert.IsNotNull(new Offer(Guid.Empty, Guid.Empty, Guid.Empty, null, null, startOfValidity, endOfValidity));
        }

        [TestMethod]
        public void SetRecommendedPrice_RejectsNegativeNumber()
        {
            var ex = Assert.ThrowsException<BaristaException>(() => new Offer(Guid.Empty, Guid.Empty, Guid.Empty, -5, null, null, null));
            Assert.AreEqual("invalid_recommended_price", ex.Code);
        }

        [TestMethod]
        public void SetRecommendedPrice_AcceptsZeroAndPositiveNumber()
        {
            foreach (var recommendedPrice in new[] {0m, 15m})
                Assert.IsNotNull(new Offer(Guid.Empty, Guid.Empty, Guid.Empty, recommendedPrice, null, null, null));
        }
    }
}
