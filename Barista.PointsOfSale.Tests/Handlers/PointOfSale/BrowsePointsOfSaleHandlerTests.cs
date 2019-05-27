using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.PointsOfSale.Dto;
using Barista.PointsOfSale.Handlers.PointOfSale;
using Barista.PointsOfSale.Queries;
using Barista.PointsOfSale.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.PointsOfSale.Tests.Handlers.PointOfSale
{
    [TestClass]
    public class BrowsePointsOfSaleHandlerTests : BrowseHandlerTestBase<BrowsePointsOfSale, IPointOfSaleRepository, Domain.PointOfSale, PointOfSaleDto>
    {
        protected override IQueryHandler<BrowsePointsOfSale, IPagedResult<PointOfSaleDto>> InstantiateHandler()
            => new BrowsePointsOfSaleHandler(Repository, Mapper);
    }
}
