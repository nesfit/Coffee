using System;
using Barista.Common;
using Barista.Common.Tests;
using Barista.Identity.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.DomainClasses
{
    [TestClass]
    public class AssignmentToUserTests
    {
        [TestMethod]
        public void SetValidity_RejectsInvalidValue()
        {
            var ex = Assert.ThrowsException<BaristaException>(() => new AssignmentToUser(Guid.Empty, Guid.Empty, TestDateTimes.Year2004, TestDateTimes.Year2003, Guid.Empty, false));
            Assert.AreEqual("invalid_validity", ex.Code);
        }
    }
}
