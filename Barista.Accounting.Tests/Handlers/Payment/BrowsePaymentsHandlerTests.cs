using Barista.Accounting.Dto;
using Barista.Accounting.Handlers.Payment;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Handlers.Payment
{
    [TestClass]
    public class BrowsePaymentsHandlerTests : BrowseHandlerTestBase<BrowsePayments, IPaymentsRepository, Accounting.Domain.Payment, PaymentDto>
    {
        protected override IQueryHandler<BrowsePayments, IPagedResult<PaymentDto>> InstantiateHandler()
            => new BrowsePaymentsHandler(Repository, Mapper);
    }
}
