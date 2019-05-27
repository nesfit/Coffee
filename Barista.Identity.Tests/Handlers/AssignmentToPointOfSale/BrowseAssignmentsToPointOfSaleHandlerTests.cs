using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Handlers.AssignmentToPointOfSale;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Handlers.AssignmentToPointOfSale
{
    [TestClass]
    public class BrowseAssignmentsToPointOfSaleHandlerTests : BrowseHandlerTestBase<BrowseAssignmentsToPointOfSale, IAssignmentsRepository, Domain.AssignmentToPointOfSale, AssignmentToPointOfSaleDto>
    {
        protected override IQueryHandler<BrowseAssignmentsToPointOfSale, IPagedResult<AssignmentToPointOfSaleDto>> InstantiateHandler()
            => new BrowseAssignmentsToPointOfSaleHandler(Repository, Mapper);
    }
}
