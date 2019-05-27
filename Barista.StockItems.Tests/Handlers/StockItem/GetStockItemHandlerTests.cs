using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.StockItems.Dto;
using Barista.StockItems.Handlers.StockItem;
using Barista.StockItems.Queries;
using Barista.StockItems.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.StockItems.Tests.Handlers.StockItem
{
    [TestClass]
    public class GetStockItemHandlerTests : GetHandlerTestBase<GetStockItemHandler, GetStockItem, IStockItemRepository, Domain.StockItem, StockItemDto>
    {
        protected override GetStockItemHandler InstantiateHandler(IStockItemRepository repo, IMapper mapper)
            => new GetStockItemHandler(repo, mapper);

        protected override GetStockItem InstantiateQuery()
            => new GetStockItem(TestIds.A);

        protected override Func<GetStockItem, Expression<Func<IStockItemRepository, Task<Domain.StockItem>>>> RepositorySetup
            => q => repo => repo.GetAsync(TestIds.A);
    }
}
