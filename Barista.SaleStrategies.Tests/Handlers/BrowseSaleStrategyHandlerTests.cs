using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.SaleStrategies.Domain;
using Barista.SaleStrategies.Dto;
using Barista.SaleStrategies.Handlers.SaleStrategy;
using Barista.SaleStrategies.Queries;
using Barista.SaleStrategies.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.SaleStrategies.Tests.Handlers
{
    [TestClass]
    public class BrowseSaleStrategyHandlerTests : BrowseHandlerTestBase<BrowseSaleStrategies, ISaleStrategyRepository, SaleStrategy, SaleStrategyDto>
    {
        protected override IQueryHandler<BrowseSaleStrategies, IPagedResult<SaleStrategyDto>> InstantiateHandler()
            => new BrowseSaleStrategiesHandler(Repository, Mapper);
    }
}
