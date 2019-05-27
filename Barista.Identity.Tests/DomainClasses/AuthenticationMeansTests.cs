using System;
using Barista.Common;
using Barista.Common.Tests;
using Barista.Identity.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.DomainClasses
{
    [TestClass]
    public class AuthenticationMeansTests
    {
        [TestMethod]
        public void SetValidity_RejectsInvalidValue()
        {
            var ex = Assert.ThrowsException<BaristaException>(() => new AuthenticationMeans(Guid.Empty, "Type", "Value", TestDateTimes.Year2004, TestDateTimes.Year2003));
            Assert.AreEqual("invalid_validity", ex.Code);
        }

        [TestMethod]
        public void SetType_RejectsNullEmptyAndWhiteSpaceStrings()
        {
            foreach (var type in new[] { null, string.Empty, " " })
            {
                var ex = Assert.ThrowsException<BaristaException>(() => new AuthenticationMeans(Guid.Empty, type, "Value", TestDateTimes.Year2004, null));
                Assert.AreEqual("invalid_type", ex.Code);
            }
        }

        [TestMethod]
        public void SetValue_RejectsNullEmptyAndWhiteSpaceStrings()
        {
            foreach (var value in new[] { null, string.Empty, " " })
            {
                var ex = Assert.ThrowsException<BaristaException>(() => new AuthenticationMeans(Guid.Empty, "Type", value, TestDateTimes.Year2004, null));
                Assert.AreEqual("invalid_value", ex.Code);
            }
        }
    }
}
