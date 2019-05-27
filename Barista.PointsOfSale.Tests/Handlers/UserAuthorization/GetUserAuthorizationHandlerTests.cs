using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.PointsOfSale.Dto;
using Barista.PointsOfSale.Handlers.UserAuthorization;
using Barista.PointsOfSale.Queries;
using Barista.PointsOfSale.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.PointsOfSale.Tests.Handlers.UserAuthorization
{
    [TestClass]
    public class GetUserAuthorizationHandlerTests : GetHandlerTestBase<GetUserAuthorizationHandler, GetUserAuthorization, IUserAuthorizationRepository, Domain.UserAuthorization, UserAuthorizationDto>
    {
        protected override GetUserAuthorizationHandler InstantiateHandler(IUserAuthorizationRepository repo, IMapper mapper)
            => new GetUserAuthorizationHandler(repo, mapper);

        protected override GetUserAuthorization InstantiateQuery()
            => new GetUserAuthorization(TestIds.A, TestIds.B);

        protected override Func<GetUserAuthorization, Expression<Func<IUserAuthorizationRepository, Task<Domain.UserAuthorization>>>>
            RepositorySetup => query => repo => repo.GetAsync(TestIds.A, TestIds.B);
    }
}
