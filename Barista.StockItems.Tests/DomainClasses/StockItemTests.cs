using Barista.Common;
using Barista.StockItems.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Barista.StockItems.Tests.DomainClasses
{
    [TestClass]
    public class StockItemTests
    {
        [TestMethod]
        public void SetDisplayName_RejectsNullEmptyWhiteSpaceStrings()
        {
            foreach (var dn in new[] { null, string.Empty, " " })
            {
                var ex = Assert.ThrowsException<BaristaException>(() => new StockItem(Guid.Empty, dn, Guid.Empty));
                Assert.AreEqual("invalid_display_name", ex.Code);
            }
        }

        [TestMethod]
        public void SetDisplayName_AcceptsNonEmptyNonNullNonWhiteSpaceString()
        {
            Assert.IsNotNull(new StockItem(Guid.Empty, "Test", Guid.Empty));
        }
    }
}
