using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Domain;
using Barista.Accounting.Dto;
using Barista.Accounting.Handlers.SaleStateChange;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Handlers.SaleStateChange
{
    [TestClass]
    public class GetSaleStateChangeHandlerTests : GetInnerCollectionHandlerTestBase<GetSaleStateChangeHandler, GetSaleStateChange, ISalesRepository, Accounting.Domain.Sale, Accounting.Domain.SaleStateChange, SaleStateChangeDto>
    {
        protected override GetSaleStateChangeHandler InstantiateHandler(ISalesRepository repo, IMapper mapper)
            => new GetSaleStateChangeHandler(repo, mapper);

        protected override GetSaleStateChange InstantiateQuery()
            => new GetSaleStateChange(InnerEntityId, ParentEntityId);

        protected override Func<GetSaleStateChange, Expression<Func<ISalesRepository, Task<Accounting.Domain.Sale>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.ParentSaleId);

        protected override Accounting.Domain.Sale InstantiateDomainWithoutDesiredEntity(GetSaleStateChange query)
            => new Accounting.Domain.Sale(query.ParentSaleId, 5, 5, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);

        protected override Accounting.Domain.Sale InstantiateDomainWithDesiredEntity(GetSaleStateChange query)
        {
            var sale = InstantiateDomainWithoutDesiredEntity(query);
            sale.AddStateChange(new Accounting.Domain.SaleStateChange(query.SaleStateChangeId, "Reason", SaleState.FundsReserved, null, null));
            return sale;
        }
    }
}
