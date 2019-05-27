using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.PointsOfSale.Dto;
using Barista.PointsOfSale.Handlers.UserAuthorization;
using Barista.PointsOfSale.Queries;
using Barista.PointsOfSale.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.PointsOfSale.Tests.Handlers.UserAuthorization
{
    [TestClass]
    public class BrowseUserAuthorizationsHandlerTests : BrowseHandlerTestBase<BrowseUserAuthorizations, IUserAuthorizationRepository, Domain.UserAuthorization, UserAuthorizationDto>
    {
        protected override IQueryHandler<BrowseUserAuthorizations, IPagedResult<UserAuthorizationDto>> InstantiateHandler()
            => new BrowseUserAuthorizationsHandler(Repository, Mapper);
    }
}
