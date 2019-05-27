using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Common.Tests;
using Barista.Offers.Services;
using Barista.Offers.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Offers.Tests.Verifiers
{
    [TestClass]
    public class ProductVerifierTests : VerifierAcceptingGuidTestBase<IProductsService>
    {
        private IProductVerifier Verifier => new ProductVerifier(ServiceMock.Object);

        protected override async Task InvokeVerifier(Guid resourceId, Task<HttpResponseMessage> responseMsgTask)
        {
            ServiceMock.Setup(svc => svc.StatProduct(resourceId)).Returns(responseMsgTask).Verifiable();
            await Verifier.AssertExists(resourceId);
        }

        protected override void VerifyExpectations(Guid resourceId)
        {
            ServiceMock.Verify(svc => svc.StatProduct(resourceId));
        }
    }
}
