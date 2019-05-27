using Barista.Common;
using Barista.Common.Dispatchers;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.Contracts.Commands.AuthenticationMeans;
using Barista.Identity.Dto;
using Barista.Identity.Handlers.AuthenticationMeans;
using Barista.Identity.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Identity.Tests.Handlers.AuthenticationMeans
{
    [TestClass]
    public class ResolveAuthenticationMeansHandlerTests
    {
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(5)]
        [DataRow(-1)]
        public void WhenResolutionFails_ReturnsUnsuccessfulIdentifierResult(int resultCount)
        {
            var cmdMock = new Mock<IResolveAuthenticationMeans>(MockBehavior.Strict);
            cmdMock.SetupGet(cmd => cmd.Method).Returns("MethodName");
            cmdMock.SetupGet(cmd => cmd.Value).Returns("MeansValue");

            var qResultMock = new Mock<IPagedResult<AuthenticationMeansDto>>(MockBehavior.Strict);
            qResultMock.SetupGet(r => r.TotalResults).Returns(resultCount).Verifiable();

            var dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
            dispatcherMock.Setup(d => d.QueryAsync(It.Is<BrowseAuthenticationMeans>(q => q.Value == cmdMock.Object.Value && q.Method == cmdMock.Object.Method && q.ResultsPerPage == 1)))
                .ReturnsAsync(qResultMock.Object)
                .Verifiable();

            var handler = new ResolveAuthenticationMeansHandler(dispatcherMock.Object);

            var result = handler.HandleAsync(cmdMock.Object, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();
            Assert.IsFalse(result.Successful);
            Assert.AreEqual("authentication_means_not_found", result.ErrorCode);

            qResultMock.Verify();
            dispatcherMock.Verify();
        }

        [TestMethod]
        public void WhenResolutionSucceeds_ReturnsSuccessfulIdentifierResult_WithResolvedMeansId()
        {
            var cmdMock = new Mock<IResolveAuthenticationMeans>(MockBehavior.Strict);
            cmdMock.SetupGet(cmd => cmd.Method).Returns("MethodName");
            cmdMock.SetupGet(cmd => cmd.Value).Returns("MeansValue");

            var authMeans = new AuthenticationMeansDto {Id = TestIds.A};
            var qResultMock = new Mock<IPagedResult<AuthenticationMeansDto>>(MockBehavior.Strict);

            qResultMock.SetupGet(r => r.TotalResults).Returns(1).Verifiable();
            qResultMock.SetupGet(r => r.Items).Returns(new[] {authMeans});

            var dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
            dispatcherMock.Setup(d => d.QueryAsync(It.Is<BrowseAuthenticationMeans>(q => q.Value == cmdMock.Object.Value && q.Method == cmdMock.Object.Method && q.ResultsPerPage == 1)))
                .ReturnsAsync(qResultMock.Object)
                .Verifiable();

            var handler = new ResolveAuthenticationMeansHandler(dispatcherMock.Object);

            var result = handler.HandleAsync(cmdMock.Object, new Mock<ICorrelationContext>().Object).GetAwaiter().GetResult();
            Assert.IsTrue(result.Successful);
            Assert.IsTrue(result.Id.HasValue);
            Assert.AreEqual(authMeans.Id, result.Id.Value);

            qResultMock.Verify();
            dispatcherMock.Verify();
        }
    }
}
