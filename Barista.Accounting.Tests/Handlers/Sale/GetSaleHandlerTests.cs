using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Dto;
using Barista.Accounting.Handlers.Sale;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Handlers.Sale
{
    [TestClass]
    public class GetSaleHandlerTests : GetHandlerTestBase<GetSaleHandler, GetSale, ISalesRepository, Accounting.Domain.Sale, SaleDto>
    {
        protected override GetSaleHandler InstantiateHandler(ISalesRepository repo, IMapper mapper)
            => new GetSaleHandler(repo, mapper);

        protected override GetSale InstantiateQuery()
            => new GetSale(Guid.Empty);

        protected override Func<GetSale, Expression<Func<ISalesRepository, Task<Accounting.Domain.Sale>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.Id);
    }
}
