using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Handlers.StockOperation;
using Barista.StockOperations.Queries;
using Barista.StockOperations.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.StockOperations.Tests.Handlers.StockOperation
{
    [TestClass]
    public class GetStockOperationHandlerTests : GetHandlerTestBase<GetStockOperationHandler, GetStockOperation, IStockOperationRepository, Domain.StockOperation, StockOperationDto>
    {
        protected override GetStockOperationHandler InstantiateHandler(IStockOperationRepository repo, IMapper mapper)
             => new GetStockOperationHandler(repo, mapper);

        protected override GetStockOperation InstantiateQuery()
            => new GetStockOperation(TestIds.A);

        protected override Domain.StockOperation InstantiateDomain()
            => (Domain.StockOperation)FormatterServices.GetSafeUninitializedObject(typeof(TestStockOperation));


        protected override Func<GetStockOperation, Expression<Func<IStockOperationRepository, Task<Domain.StockOperation>>>> RepositorySetup
            => q => repo => repo.GetAsync(TestIds.A);
    }
}
