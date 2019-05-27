using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.Identity.Dto;
using Barista.Identity.Handlers.AuthenticationMeans;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Handlers.AuthenticationMeans
{
    [TestClass]
    public class GetAuthenticationMeansHandlerTests : GetHandlerTestBase<GetAuthenticationMeansHandler, GetAuthenticationMeans, IAuthenticationMeansRepository, Domain.AuthenticationMeans, AuthenticationMeansDto>
    {
        protected override GetAuthenticationMeansHandler InstantiateHandler(IAuthenticationMeansRepository repo, IMapper mapper)
            => new GetAuthenticationMeansHandler(repo, mapper);

        protected override GetAuthenticationMeans InstantiateQuery()
            => new GetAuthenticationMeans(TestIds.A);

        protected override Func<GetAuthenticationMeans, Expression<Func<IAuthenticationMeansRepository, Task<Domain.AuthenticationMeans>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.Id);
    }
}
