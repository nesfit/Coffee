using System;
using Barista.Common;
using Barista.Identity.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.DomainClasses
{
    [TestClass]
    public class SpendingLimitTests
    {
        [TestMethod]
        public void SetInterval_RejectsNegativeInterval()
        {
            var ex = Assert.ThrowsException<BaristaException>(() => new SpendingLimit(Guid.Empty, TimeSpan.FromHours(-5), 10));
            Assert.AreEqual("invalid_interval", ex.Code);
        }

        [TestMethod]
        public void SetValue_RejectsNegativeValue()
        {
            var ex = Assert.ThrowsException<BaristaException>(() => new SpendingLimit(Guid.Empty, TimeSpan.FromHours(1), -5));
            Assert.AreEqual("invalid_value", ex.Code);
        }
    }
}
