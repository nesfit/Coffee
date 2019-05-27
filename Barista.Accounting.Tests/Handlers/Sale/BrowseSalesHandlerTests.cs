using Barista.Accounting.Dto;
using Barista.Accounting.Handlers.Sale;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Handlers.Sale
{
    [TestClass]
    public class BrowseSalesHandlerTests : BrowseHandlerTestBase<BrowseSales, ISalesRepository, Accounting.Domain.Sale, SaleDto>
    {
        protected override IQueryHandler<BrowseSales, IPagedResult<SaleDto>> InstantiateHandler()
            => new BrowseSalesHandler(Repository, Mapper);
    }
}
