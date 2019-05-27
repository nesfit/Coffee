using System;
using Barista.AccountingGroups.Domain;
using Barista.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.AccountingGroups.Tests.Domain
{
    [TestClass]
    public class AccountingGroupTests
    {
        [TestMethod]
        public void SetDisplayName_RejectsNullEmptyAndWhiteSpaceStrings()
        {
            foreach (var displayName in new[] {null, string.Empty, " "})
            {
                var e = Assert.ThrowsException<BaristaException>(() => new AccountingGroup(Guid.Empty, displayName, Guid.Empty));
                Assert.AreEqual(e.Code, "invalid_display_name");
            }
        }
    }
}
