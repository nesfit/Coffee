using AutoMapper;
using Barista.AccountingGroups.Domain;
using Barista.AccountingGroups.Dto;
using Barista.AccountingGroups.Handlers.AccountingGroup;
using Barista.AccountingGroups.Queries;
using Barista.AccountingGroups.Repositories;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Barista.AccountingGroups.Tests.Handlers.AccountingGroup
{
    [TestClass]
    public class GetAccountingGroupHandlerTests : GetHandlerTestBase<GetAccountingGroupHandler, GetAccountingGroup, IAccountingGroupRepository, AccountingGroups.Domain.AccountingGroup, AccountingGroupDto>
    {
        protected override Func<GetAccountingGroup, Expression<Func<IAccountingGroupRepository, Task<AccountingGroups.Domain.AccountingGroup>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.Id);

        protected override GetAccountingGroupHandler InstantiateHandler(IAccountingGroupRepository repo, IMapper mapper)
            => new GetAccountingGroupHandler(repo, mapper);

        protected override GetAccountingGroup InstantiateQuery()
            => new GetAccountingGroup(Guid.Empty);
    }
}
