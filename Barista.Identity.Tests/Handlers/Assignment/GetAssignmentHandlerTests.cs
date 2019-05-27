using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.Identity.Dto;
using Barista.Identity.Handlers.Assignment;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Handlers.Assignment
{
    [TestClass]
    public class GetAssignmentHandlerTests : GetHandlerTestBase<GetAssignmentHandler, GetAssignment, IAssignmentsRepository, Domain.Assignment, AssignmentDto>
    {
        protected override GetAssignmentHandler InstantiateHandler(IAssignmentsRepository repo, IMapper mapper)
            => new GetAssignmentHandler(repo, mapper);

        protected override GetAssignment InstantiateQuery()
            => new GetAssignment(TestIds.A);

        protected override Domain.Assignment InstantiateDomain()
            =>  (Domain.Assignment) FormatterServices.GetSafeUninitializedObject(typeof(Domain.AssignmentToUser));

        protected override Func<GetAssignment, Expression<Func<IAssignmentsRepository, Task<Domain.Assignment>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.Id);
    }
}
