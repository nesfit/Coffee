using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Dto;
using Barista.Accounting.Handlers.Balance;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Handlers.Balance
{
    [TestClass]
    public class GetBalanceHandlerTests : GetHandlerTestBase<GetBalanceHandler, GetBalance, IBalanceRepository, Barista.Accounting.Domain.Balance, BalanceDto>
    {
        protected override GetBalanceHandler InstantiateHandler(IBalanceRepository repo, IMapper mapper)
            => new GetBalanceHandler(repo, mapper);

        protected override GetBalance InstantiateQuery()
            => new GetBalance(Guid.Empty);

        protected override Func<GetBalance, Expression<Func<IBalanceRepository, Task<Barista.Accounting.Domain.Balance>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.UserId);
    }
}
