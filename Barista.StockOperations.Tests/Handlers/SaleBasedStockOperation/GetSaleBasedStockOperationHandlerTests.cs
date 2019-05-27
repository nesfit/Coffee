using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Handlers.SaleBasedStockOperation;
using Barista.StockOperations.Queries;
using Barista.StockOperations.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.StockOperations.Tests.Handlers.SaleBasedStockOperation
{
    [TestClass]
    public class GetSaleBasedStockOperationHandlerTests : GetHandlerTestBase<GetSaleBasedStockOperationHandler, GetSaleBasedStockOperation, IStockOperationRepository, Domain.StockOperation, SaleBasedStockOperationDto>
    {
        protected override GetSaleBasedStockOperationHandler InstantiateHandler(IStockOperationRepository repo, IMapper mapper)
            => new GetSaleBasedStockOperationHandler(repo, mapper);

        protected override GetSaleBasedStockOperation InstantiateQuery()
            => new GetSaleBasedStockOperation(TestIds.A);

        protected override Domain.StockOperation InstantiateDomain()
            => (Domain.StockOperation)FormatterServices.GetSafeUninitializedObject(typeof(Domain.SaleBasedStockOperation));
        
        protected override Func<GetSaleBasedStockOperation, Expression<Func<IStockOperationRepository, Task<Domain.StockOperation>>>> RepositorySetup
            => q => repo => repo.GetAsync(TestIds.A);
    }
}
