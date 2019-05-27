using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.Users.Dto;
using Barista.Users.Handlers.User;
using Barista.Users.Queries;
using Barista.Users.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Users.Tests.Handlers.User
{
    [TestClass]
    public class BrowseUsersHandlerTests : BrowseHandlerTestBase<BrowseUsers, IUserRepository, Domain.User, UserDto>
    {
        protected override IQueryHandler<BrowseUsers, IPagedResult<UserDto>> InstantiateHandler()
            => new BrowseUsersHandler(Repository, Mapper);
    }
}
