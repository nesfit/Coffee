using System;
using Barista.AccountingGroups.Repositories;
using Barista.AccountingGroups.Verifiers;
using Barista.Common;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.AccountingGroups.Tests.Verifiers
{
    [TestClass]
    public class AccountingGroupVerifierTests
    {
        private readonly Mock<IAccountingGroupRepository> _repositoryMock = new Mock<IAccountingGroupRepository>(MockBehavior.Strict);
        private IAccountingGroupVerifier Verifier => new AccountingGroupVerifier(_repositoryMock.Object);

        [TestMethod]
        public void CallsRepositoryToVerifyExistence_DoesNotThrowWhenResourceExists()
        {
            _repositoryMock.Setup(r => r.GetAsync(TestIds.A)).ReturnsAsync(new Barista.AccountingGroups.Domain.AccountingGroup(Guid.Empty, "Test", Guid.Empty)).Verifiable();
            Verifier.AssertExists(TestIds.A).GetAwaiter().GetResult();
            _repositoryMock.Verify();
        }

        [TestMethod]
        public void CallsRepositoryToVerifyExistence_ThrowsBaristaExceptionWhenResourceDoesNotExist_WithCorrectCode_WithResourceIdInMessage()
        {
            var resourceId = TestIds.A;

            _repositoryMock.Setup(r => r.GetAsync(resourceId)).ReturnsAsync((Barista.AccountingGroups.Domain.AccountingGroup)null).Verifiable();
            var e = Assert.ThrowsException<BaristaException>(() => Verifier.AssertExists(resourceId).GetAwaiter().GetResult());
            Assert.IsTrue(e.Code.EndsWith("not_found"));
            Assert.IsTrue(e.Message.Contains(resourceId.ToString()));
            _repositoryMock.Verify();
        }
    }
}
