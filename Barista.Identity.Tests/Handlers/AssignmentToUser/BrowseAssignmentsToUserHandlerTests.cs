using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Handlers.AssignmentToUser;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Handlers.AssignmentToUser
{
    [TestClass]
    public class BrowseAssignmentsToUserHandlerTests : BrowseHandlerTestBase<BrowseAssignmentsToUser, IAssignmentsRepository, Domain.AssignmentToUser, AssignmentToUserDto>
    {
        protected override IQueryHandler<BrowseAssignmentsToUser, IPagedResult<AssignmentToUserDto>> InstantiateHandler()
            => new BrowseAssignmentsToUserHandler(Repository, Mapper);
    }
}
