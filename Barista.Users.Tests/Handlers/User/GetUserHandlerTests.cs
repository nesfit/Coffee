using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.Users.Dto;
using Barista.Users.Handlers.User;
using Barista.Users.Queries;
using Barista.Users.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Users.Tests.Handlers.User
{
    [TestClass]
    public class GetUserHandlerTests : GetHandlerTestBase<GetUserHandler, GetUser, IUserRepository, Domain.User, UserDto>
    {
        protected override GetUserHandler InstantiateHandler(IUserRepository repo, IMapper mapper)
            => new GetUserHandler(repo, mapper);

        protected override GetUser InstantiateQuery()
            => new GetUser(TestIds.A);

        protected override Func<GetUser, Expression<Func<IUserRepository, Task<Domain.User>>>> RepositorySetup
            => q => repo => repo.GetAsync(TestIds.A);
    }
}
