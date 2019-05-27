using AutoMapper;
using Barista.AccountingGroups.Dto;
using Barista.AccountingGroups.Handlers.UserAuthorization;
using Barista.AccountingGroups.Queries;
using Barista.AccountingGroups.Repositories;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Barista.AccountingGroups.Tests.Handlers.UserAuthorization
{
    [TestClass]
    class GetUserAuthorizationHandlerTests : GetHandlerTestBase<GetUserAuthorizationHandler, GetUserAuthorization, IUserAuthorizationRepository, AccountingGroups.Domain.UserAuthorization, UserAuthorizationDto>
    {
        protected override Func<GetUserAuthorization, Expression<Func<IUserAuthorizationRepository, Task<AccountingGroups.Domain.UserAuthorization>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.AccountingGroupId, query.UserId);

        protected override GetUserAuthorizationHandler InstantiateHandler(IUserAuthorizationRepository repo, IMapper mapper)
            => new GetUserAuthorizationHandler(repo, mapper);

        protected override GetUserAuthorization InstantiateQuery()
            => new GetUserAuthorization(TestIds.A, TestIds.B);
    }
}
