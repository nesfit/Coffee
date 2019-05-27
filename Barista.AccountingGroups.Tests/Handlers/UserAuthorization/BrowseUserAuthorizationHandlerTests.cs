using Barista.AccountingGroups.Dto;
using Barista.AccountingGroups.Handlers.UserAuthorization;
using Barista.AccountingGroups.Queries;
using Barista.AccountingGroups.Repositories;
using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.AccountingGroups.Tests.Handlers.UserAuthorization
{
    [TestClass]
    public class BrowseUserAuthorizationHandlerTests : BrowseHandlerTestBase<BrowseUserAuthorizations, IUserAuthorizationRepository, AccountingGroups.Domain.UserAuthorization, Dto.UserAuthorizationDto>
    {
        protected override IQueryHandler<BrowseUserAuthorizations, IPagedResult<UserAuthorizationDto>> InstantiateHandler()
            => new BrowseUserAuthorizationsHandler(Repository, Mapper);
    }
}
