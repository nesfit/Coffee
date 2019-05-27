using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Dto;
using Barista.Accounting.Handlers.SaleStateChange;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Handlers.SaleStateChange
{
    [TestClass]
    public class BrowseSaleStateChangesHandlerTests : BrowseInnerCollectionHandlerTestBase<BrowseSaleStateChangesHandler, BrowseSaleStateChanges, ISalesRepository, Accounting.Domain.Sale, Accounting.Domain.SaleStateChange, SaleStateChangeDto>
    {
        protected override BrowseSaleStateChangesHandler InstantiateHandler(ISalesRepository repo, IMapper mapper, IClientsidePaginator<Accounting.Domain.SaleStateChange> paginator)
            => new BrowseSaleStateChangesHandler(repo, mapper, paginator);

        protected override BrowseSaleStateChanges InstantiateQuery()
            => new BrowseSaleStateChanges(ParentEntityId);

        protected override Func<BrowseSaleStateChanges, Expression<Func<ISalesRepository, Task<Accounting.Domain.Sale>>>> RepositorySetup
            => q => repo => repo.GetAsync(q.ParentSaleId);

        protected override Accounting.Domain.Sale InstantiateDomain(BrowseSaleStateChanges query, out ICollection<Accounting.Domain.SaleStateChange> innerCollection)
        {
            var sale = new Accounting.Domain.Sale(Guid.Empty, 5, 5, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
            innerCollection = sale.StateChanges;
            return sale;
        }
    }
}
