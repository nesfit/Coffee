using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Common.Tests;
using Barista.Identity.Dto;
using Barista.Identity.Handlers.SpendingLimit;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Handlers.SpendingLimit
{
    [TestClass]
    public class BrowseSpendingLimitsHandlerTests : BrowseInnerCollectionHandlerTestBase<BrowseSpendingLimitsHandler, BrowseSpendingLimits, IAssignmentsRepository, Domain.Assignment, Domain.SpendingLimit, SpendingLimitDto>
    {
        protected override BrowseSpendingLimitsHandler InstantiateHandler(IAssignmentsRepository repo, IMapper mapper, IClientsidePaginator<Domain.SpendingLimit> paginator)
            => new BrowseSpendingLimitsHandler(repo, paginator, mapper);

        protected override BrowseSpendingLimits InstantiateQuery()
            => new BrowseSpendingLimits {ParentAssignmentToUserId = ParentEntityId};

        protected override Func<BrowseSpendingLimits, Expression<Func<IAssignmentsRepository, Task<Domain.Assignment>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.ParentAssignmentToUserId);

        protected override Domain.Assignment InstantiateDomain(BrowseSpendingLimits query, out ICollection<Domain.SpendingLimit> innerCollection)
        {
            var assignment = new Domain.AssignmentToUser(TestIds.A, Guid.Empty, TestDateTimes.Year2001, null, TestIds.B, false);
            innerCollection = assignment.SpendingLimits;
            return assignment;
        }
    }
}
