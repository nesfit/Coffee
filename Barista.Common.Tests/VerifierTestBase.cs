using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Common.Tests
{
    public abstract class VerifierTestBase<TId, TService> where TService : class
    {
        protected Task<HttpResponseMessage> GetResourceExistsResponse()
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));

        protected Task<HttpResponseMessage> GetResourceNotFoundResponse()
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));

        protected Mock<TService> ServiceMock { get; } = new Mock<TService>();
        protected abstract TId ResourceId { get; }
        protected abstract Task InvokeVerifier(TId resourceId, Task<HttpResponseMessage> httpResponseMsgTask);
        protected abstract void VerifyExpectations(TId resourceId);

        [TestMethod]
        public void WhenResourceIsNotFound_ThrowsBaristaException_WithNotFoundCode_WithMessageContainingResourceId()
        {
            var e = Assert.ThrowsException<BaristaException>(() => InvokeVerifier(ResourceId, GetResourceNotFoundResponse()).GetAwaiter().GetResult());
            Assert.IsTrue(e.Code.Contains("not_found"));
            Assert.IsTrue(e.Message.Contains(ResourceId.ToString()));
            VerifyExpectations(ResourceId);
        }

        [TestMethod]
        public void WhenResourceIsFound_DoesNotThrowException()
        {
            InvokeVerifier(ResourceId, GetResourceExistsResponse()).GetAwaiter().GetResult();
            VerifyExpectations(ResourceId);
        }
    }
}
