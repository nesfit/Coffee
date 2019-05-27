using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.AccountingGroups.Services;
using Barista.AccountingGroups.Verifiers;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.AccountingGroups.Tests.Verifiers
{
    [TestClass]
    public class UserVerifierTests : VerifierAcceptingGuidTestBase<IUsersService>
    {
        private IUserVerifier Verifier => new UserVerifier(ServiceMock.Object);

        protected override async Task InvokeVerifier(Guid resourceId, Task<HttpResponseMessage> responseMsgTask)
        {
            ServiceMock.Setup(svc => svc.StatUser(resourceId)).Returns(responseMsgTask).Verifiable();
            await Verifier.AssertExists(resourceId);
        }

        protected override void VerifyExpectations(Guid resourceId)
        {
            ServiceMock.Verify(svc => svc.StatUser(resourceId));
        }
    }
}
