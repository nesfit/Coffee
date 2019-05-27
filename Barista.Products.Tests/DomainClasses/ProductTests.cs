using Barista.Common;
using Barista.Products.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Barista.Products.Tests.DomainClasses
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void SetDisplayName_RejectsNullEmptyWhiteSpaceStrings()
        {
            foreach (var dn in new[] { null, string.Empty, " " })
            {
                var ex = Assert.ThrowsException<BaristaException>(() => new Product(Guid.Empty, dn, null));
                Assert.AreEqual("invalid_display_name", ex.Code);
            }
        }

        [TestMethod]
        public void SetDisplayName_AcceptsNonEmptyNonNullNonWhiteSpaceString()
        {
            Assert.IsNotNull(new Product(Guid.Empty, "Test", null));
        }

        [TestMethod]
        public void SetRecommendedPrice_RejectsNegativeValue()
        {
            var ex = Assert.ThrowsException<BaristaException>(() => new Product(Guid.Empty, "Product name", -10));
            Assert.AreEqual("invalid_recommended_price", ex.Code);
        }

        [TestMethod]
        public void SetRecommendedPrice_AcceptsNullValue()
        {
            Assert.IsNotNull(new Product(Guid.Empty, "Product name", null));
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(10)]
        public void SetRecommendedPrice_AcceptsZeroAndPositiveValues(int recommendedPrice)
        {
            Assert.IsNotNull(new Product(Guid.Empty, "Product name", recommendedPrice));
        }
    }
}
