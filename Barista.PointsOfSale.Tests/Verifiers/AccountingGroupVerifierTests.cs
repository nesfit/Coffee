using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Common.Tests;
using Barista.PointsOfSale.Services;
using Barista.PointsOfSale.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.PointsOfSale.Tests.Verifiers
{
    [TestClass]
    public class AccountingGroupVerifierTests : VerifierAcceptingGuidTestBase<IAccountingGroupsService>
    {
        private IAccountingGroupVerifier Verifier => new AccountingGroupVerifier(ServiceMock.Object);

        protected override async Task InvokeVerifier(Guid resourceId, Task<HttpResponseMessage> responseMsgTask)
        {
            ServiceMock.Setup(svc => svc.StatAccountingGroup(resourceId)).Returns(responseMsgTask).Verifiable();
            await Verifier.AssertExists(resourceId);
        }

        protected override void VerifyExpectations(Guid resourceId)
        {
            ServiceMock.Verify(svc => svc.StatAccountingGroup(resourceId));
        }
    }
}
