using System;
using Barista.Common;
using Barista.Users.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Users.Tests.DomainClasses
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void SetFullName_RejectsNullEmptyWhiteSpaceStrings()
        {
            foreach (var fullName in new[] { null, string.Empty, " " })
            {
                var ex = Assert.ThrowsException<BaristaException>(() => new User(Guid.Empty, fullName, "user@example.com", false));
                Assert.AreEqual("invalid_full_name", ex.Code);
            }
        }

        [TestMethod]
        public void SetFullName_AcceptsNonEmptyNonNullNonWhiteSpaceString()
        {
            Assert.IsNotNull(new User(Guid.Empty, "Firstname Lastname","user@example.com", false));
        }

        [TestMethod]
        public void SetEmailAddress_RejectsNullEmptyWhiteSpaceStrings()
        {
            foreach (var emailAddress in new[] { null, string.Empty, " " })
            {
                var ex = Assert.ThrowsException<BaristaException>(() => new User(Guid.Empty, "Firstname Lastname", emailAddress, false));
                Assert.AreEqual("invalid_email_address", ex.Code);
            }
        }

        [TestMethod]
        public void SetEmailAddress_AcceptsNonEmptyNonNullNonWhiteSpaceString()
        {
            Assert.IsNotNull(new User(Guid.Empty, "Firstname Lastname", "user@example.com", false));
        }
    }
}