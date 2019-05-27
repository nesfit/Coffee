using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.SaleStrategies.Domain;
using Barista.SaleStrategies.Dto;
using Barista.SaleStrategies.Handlers.SaleStrategy;
using Barista.SaleStrategies.Queries;
using Barista.SaleStrategies.Repositories;
using Barista.SaleStrategies.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.SaleStrategies.Tests.Handlers
{
    [TestClass]
    public class GetSaleStrategyHandlerTests : GetHandlerTestBase<GetSaleStrategyHandler, GetSaleStrategy, ISaleStrategyRepository, SaleStrategy, SaleStrategyDto>
    {
        private class TestSaleStrategy : SaleStrategy
        {
            public TestSaleStrategy() : base(Guid.Empty, "TestStrategy")
            {

            }

            public override Task<bool> ApplyAsync(IAccountingService accountingService, Guid userId, decimal cost)
            {
                throw new NotImplementedException();
            }
        }

        protected override GetSaleStrategyHandler InstantiateHandler(ISaleStrategyRepository repo, IMapper mapper)
            => new GetSaleStrategyHandler(repo, mapper);

        protected override GetSaleStrategy InstantiateQuery()
            => new GetSaleStrategy(TestIds.A);

        protected override SaleStrategy InstantiateDomain()
            => new TestSaleStrategy();

        protected override Func<GetSaleStrategy, Expression<Func<ISaleStrategyRepository, Task<SaleStrategy>>>> RepositorySetup
            => query => repo => repo.GetSaleStrategy(TestIds.A);
    }
}
