using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.PointsOfSale.Dto;
using Barista.PointsOfSale.Handlers.PointOfSale;
using Barista.PointsOfSale.Queries;
using Barista.PointsOfSale.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.PointsOfSale.Tests.Handlers.PointOfSale
{
    [TestClass]
    public class GetPointOfSaleHandlerTests : GetHandlerTestBase<GetPointOfSaleHandler, GetPointOfSale, IPointOfSaleRepository, Domain.PointOfSale, PointOfSaleDto>
    {
        protected override GetPointOfSaleHandler InstantiateHandler(IPointOfSaleRepository repo, IMapper mapper)
            => new GetPointOfSaleHandler(repo, mapper);

        protected override GetPointOfSale InstantiateQuery()
            => new GetPointOfSale(TestIds.A);

        protected override Func<GetPointOfSale, Expression<Func<IPointOfSaleRepository, Task<Domain.PointOfSale>>>> RepositorySetup
            => query => repo => repo.GetAsync(TestIds.A);
    }
}
