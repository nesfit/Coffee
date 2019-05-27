using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Common.Tests;
using Barista.StockOperations.Services;
using Barista.StockOperations.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.StockOperations.Tests.Verifiers
{
    [TestClass]
    public class StockItemVerifierTests : VerifierAcceptingGuidTestBase<IStockItemsService>
    {
        protected override async Task InvokeVerifier(Guid resourceId, Task<HttpResponseMessage> httpResponseMsgTask)
        {
            ServiceMock.Setup(sis => sis.StatStockItem(resourceId)).Returns(httpResponseMsgTask).Verifiable();
            var verifier = new StockItemVerifier(ServiceMock.Object);
            await verifier.AssertExists(resourceId);
        }

        protected override void VerifyExpectations(Guid resourceId)
        {
            ServiceMock.Verify();
        }
    }
}
