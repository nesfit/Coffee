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
    public class SaleStrategyVerifierTests : VerifierAcceptingGuidTestBase<ISaleStrategiesService>
    {
        private ISaleStrategyVerifier Verifier => new SaleStrategyVerifier(ServiceMock.Object);

        protected override async Task InvokeVerifier(Guid resourceId, Task<HttpResponseMessage> responseMsgTask)
        {
            ServiceMock.Setup(svc => svc.StatSaleStrategy(resourceId)).Returns(responseMsgTask).Verifiable();
            await Verifier.AssertExists(resourceId);
        }

        protected override void VerifyExpectations(Guid resourceId)
        {
            ServiceMock.Verify(svc => svc.StatSaleStrategy(resourceId));
        }
    }
}
