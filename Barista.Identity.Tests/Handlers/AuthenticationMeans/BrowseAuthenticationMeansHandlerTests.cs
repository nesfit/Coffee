using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Handlers.AuthenticationMeans
{
    [TestClass]
    public class BrowseAuthenticationMeansHandlerTests : BrowseHandlerTestBase<BrowseAuthenticationMeans, IAuthenticationMeansRepository, Domain.AuthenticationMeans, AuthenticationMeansDto>
    {
        protected override IQueryHandler<BrowseAuthenticationMeans, IPagedResult<AuthenticationMeansDto>> InstantiateHandler()
            => new Identity.Handlers.AuthenticationMeans.BrowseAuthenticationMeansHandler(Repository, Mapper);
    }
}
