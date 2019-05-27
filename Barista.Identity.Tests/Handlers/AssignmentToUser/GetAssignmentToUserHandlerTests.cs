using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.Identity.Dto;
using Barista.Identity.Handlers.AssignmentToUser;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Handlers.AssignmentToUser
{
    [TestClass]
    public class GetAssignmentHandlerTests : GetHandlerTestBase<GetAssignmentToUserHandler, GetAssignmentToUser, IAssignmentsRepository, Domain.Assignment, AssignmentToUserDto>
    {
        protected override GetAssignmentToUserHandler InstantiateHandler(IAssignmentsRepository repo, IMapper mapper)
            => new GetAssignmentToUserHandler(repo, mapper);

        protected override GetAssignmentToUser InstantiateQuery()
            => new GetAssignmentToUser(TestIds.A);

        protected override Domain.Assignment InstantiateDomain()
            => (Domain.AssignmentToUser)FormatterServices.GetSafeUninitializedObject(typeof(Domain.AssignmentToUser));

        protected override Func<GetAssignmentToUser, Expression<Func<IAssignmentsRepository, Task<Domain.Assignment>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.Id);
    }
}
