using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.Identity.Dto;
using Barista.Identity.Handlers.SpendingLimit;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Handlers.SpendingLimit
{
    [TestClass]
    public class GetSpendingLimitHandlerTests : GetInnerCollectionHandlerTestBase<GetSpendingLimitHandler, GetSpendingLimit, IAssignmentsRepository, Domain.Assignment, Domain.SpendingLimit, SpendingLimitDto>
    {
        protected override GetSpendingLimitHandler InstantiateHandler(IAssignmentsRepository repo, IMapper mapper)
            => new GetSpendingLimitHandler(repo, mapper);

        protected override GetSpendingLimit InstantiateQuery()
            => new GetSpendingLimit(TestIds.B,ParentEntityId);

        protected override Func<GetSpendingLimit, Expression<Func<IAssignmentsRepository, Task<Domain.Assignment>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.ParentAssignmentToUserId);

        protected override Domain.Assignment InstantiateDomainWithoutDesiredEntity(GetSpendingLimit query)
            => new Domain.AssignmentToUser(TestIds.A, Guid.Empty, TestDateTimes.Year2001, null, Guid.Empty, false);

        protected override Domain.Assignment InstantiateDomainWithDesiredEntity(GetSpendingLimit query)
        {
            var assignment = (Domain.AssignmentToUser)InstantiateDomainWithoutDesiredEntity(query);
            assignment.SpendingLimits.Add(new Domain.SpendingLimit(TestIds.B, TimeSpan.FromHours(5), 10));
            return assignment;
        }
    }
}
