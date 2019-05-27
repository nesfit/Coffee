using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Dto;
using Barista.Accounting.Handlers.Payment;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Handlers.Payment
{
    [TestClass]
    public class GetPaymentHandlerTests : GetHandlerTestBase<GetPaymentHandler, GetPayment, IPaymentsRepository, Accounting.Domain.Payment, PaymentDto>
    {
        protected override GetPaymentHandler InstantiateHandler(IPaymentsRepository repo, IMapper mapper)
            => new GetPaymentHandler(repo, mapper);

        protected override GetPayment InstantiateQuery()
            => new GetPayment(Guid.Empty);

        protected override Func<GetPayment, Expression<Func<IPaymentsRepository, Task<Accounting.Domain.Payment>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.Id);
    }
}
