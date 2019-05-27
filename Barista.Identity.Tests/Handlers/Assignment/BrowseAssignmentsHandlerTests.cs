using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Handlers.Assignment;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;

namespace Barista.Identity.Tests.Handlers.Assignment
{
    public class BrowseAssignmentsHandlerTests : BrowseHandlerTestBase<BrowseAssignments, IAssignmentsRepository, Domain.Assignment, AssignmentDto>
    {
        protected override IQueryHandler<BrowseAssignments, IPagedResult<AssignmentDto>> InstantiateHandler()
            => new BrowseAssignmentsHandler(Repository, Mapper);
    }
}
