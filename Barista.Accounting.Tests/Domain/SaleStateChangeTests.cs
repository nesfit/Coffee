using System;
using Barista.Accounting.Domain;
using Barista.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Domain
{
    [TestClass]
    public class SaleStateChangeTests
    {
        [TestMethod]
        public void SetReason_RejectsNullEmptyAndWhiteSpaceStrings()
        {
            foreach (var reason in new[] {null, string.Empty, " "})
            {
                var ex = Assert.ThrowsException<BaristaException>(() => new SaleStateChange(Guid.Empty, reason, SaleState.Cancelled, null, null));
                Assert.AreEqual("invalid_reason", ex.Code);
            }
        }
    }
}
