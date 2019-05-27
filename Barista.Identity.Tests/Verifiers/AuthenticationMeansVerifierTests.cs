using System;
using Barista.Common;
using Barista.Common.Tests;
using Barista.Identity.Repositories;
using Barista.Identity.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Identity.Tests.Verifiers
{
    [TestClass]
    public class AuthenticationMeansVerifierTests
    {
        private readonly Mock<IAuthenticationMeansRepository> _repositoryMock = new Mock<IAuthenticationMeansRepository>(MockBehavior.Strict);
        private IAuthenticationMeansVerifier Verifier => new AuthenticationMeansVerifier(_repositoryMock.Object);

        [TestMethod]
        public void CallsRepositoryToVerifyExistence_DoesNotThrowWhenResourceExists()
        {
            _repositoryMock.Setup(r => r.GetAsync(TestIds.A)).ReturnsAsync(new Domain.AuthenticationMeans(Guid.Empty, "Type", "Value", TestDateTimes.Year2001, null)).Verifiable();
            Verifier.AssertExists(TestIds.A).GetAwaiter().GetResult();
            _repositoryMock.Verify();
        }

        [TestMethod]
        public void CallsRepositoryToVerifyExistence_ThrowsBaristaExceptionWhenResourceDoesNotExist_WithCorrectCode_WithResourceIdInMessage()
        {
            var resourceId = TestIds.A;

            _repositoryMock.Setup(r => r.GetAsync(resourceId)).ReturnsAsync((Domain.AuthenticationMeans)null).Verifiable();
            var e = Assert.ThrowsException<BaristaException>(() => Verifier.AssertExists(resourceId).GetAwaiter().GetResult());
            Assert.IsTrue(e.Code.EndsWith("not_found"));
            Assert.IsTrue(e.Message.Contains(resourceId.ToString()));
            _repositoryMock.Verify();
        }
    }
}
