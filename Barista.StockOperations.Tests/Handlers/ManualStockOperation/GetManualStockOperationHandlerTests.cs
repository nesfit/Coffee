using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Handlers.ManualStockOperation;
using Barista.StockOperations.Queries;
using Barista.StockOperations.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.StockOperations.Tests.Handlers.ManualStockOperation
{
    [TestClass]
    public class GetManualStockOperationHandlerTests : GetHandlerTestBase<GetManualStockOperationHandler, GetManualStockOperation, IStockOperationRepository, Domain.StockOperation, ManualStockOperationDto>
    {
        protected override GetManualStockOperationHandler InstantiateHandler(IStockOperationRepository repo, IMapper mapper)
            => new GetManualStockOperationHandler(repo, mapper);

        protected override GetManualStockOperation InstantiateQuery()
            => new GetManualStockOperation(TestIds.A);

        protected override Domain.StockOperation InstantiateDomain()
            => (Domain.StockOperation) FormatterServices.GetSafeUninitializedObject(typeof(Domain.ManualStockOperation));

        protected override Func<GetManualStockOperation, Expression<Func<IStockOperationRepository, Task<Domain.StockOperation>>>> RepositorySetup
            => q => repo => repo.GetAsync(TestIds.A);
    }
}
