using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Accounting.Services;
using Barista.Accounting.Verifiers;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Verifiers
{
    [TestClass]
    public class AuthenticationMeansVerifierTests : VerifierAcceptingGuidTestBase<IIdentityService>
    {
        private IAuthenticationMeansVerifier Verifier => new AuthenticationMeansVerifier(ServiceMock.Object);

        protected override async Task InvokeVerifier(Guid resourceId, Task<HttpResponseMessage> responseMsgTask)
        {
            ServiceMock.Setup(svc => svc.StatAuthenticationMeans(resourceId)).Returns(responseMsgTask).Verifiable();
            await Verifier.AssertExists(resourceId);
        }

        protected override void VerifyExpectations(Guid resourceId)
        {
            ServiceMock.Verify(svc => svc.StatAuthenticationMeans(resourceId));
        }
    }
}
