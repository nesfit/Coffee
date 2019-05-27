using System;
using Barista.Accounting.Domain;
using Barista.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Domain
{
    [TestClass]
    public class PaymentTests
    {
        [TestMethod]
        public void SetAmount_RejectsNegativeNumbersAndZero()
        {
            foreach (var amount in new[] { -10m, 0m })
            {
                var ex = Assert.ThrowsException<BaristaException>(() => new Payment(Guid.Empty, amount, Guid.Empty, "Source", "ExtId"));
                Assert.AreEqual(ex.Code, "invalid_amount");
            }
        }

        [TestMethod]
        public void SetSource_RejectsNullEmptyAndWhiteSpaceStrings()
        {
            foreach (var source in new[] { null, string.Empty, " " })
            {
                var ex = Assert.ThrowsException<BaristaException>(() => new Payment(Guid.Empty, 5, Guid.Empty, source, "ExtId"));
                Assert.AreEqual(ex.Code, "invalid_source");
            }
        }
    }
}
