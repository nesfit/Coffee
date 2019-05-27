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
