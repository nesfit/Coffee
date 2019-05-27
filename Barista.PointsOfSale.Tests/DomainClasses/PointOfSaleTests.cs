using Barista.Common;
using Barista.PointsOfSale.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Barista.PointsOfSale.Tests.DomainClasses
{
    [TestClass]
    public class PointOfSaleTests
    {
        [TestMethod]
        public void SetDisplayName_RejectsNullEmptyWhiteSpaceStrings()
        {
            foreach (var dn in new[] { null, string.Empty, " " })
            {
                var ex = Assert.ThrowsException<BaristaException>(() => new PointOfSale(Guid.Empty, dn, Guid.Empty, null));
                Assert.AreEqual("invalid_display_name", ex.Code);
            }
        }

        [TestMethod]
        public void SetDisplayName_AcceptsNonEmptyNonNullNonWhiteSpaceString()
        {
            Assert.IsNotNull(new PointOfSale(Guid.Empty, "Test", Guid.Empty, null));
        }
    }
}
