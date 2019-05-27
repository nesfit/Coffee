using Barista.AccountingGroups.Dto;
using Barista.AccountingGroups.Handlers.AccountingGroup;
using Barista.AccountingGroups.Queries;
using Barista.AccountingGroups.Repositories;
using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Barista.AccountingGroups.Tests.Handlers.AccountingGroup
{
    [TestClass]
    public class BrowseAccountingGroupsHandlerTests : BrowseHandlerTestBase<BrowseAccountingGroups, IAccountingGroupRepository, Barista.AccountingGroups.Domain.AccountingGroup, AccountingGroupDto>
    {
        protected override IQueryHandler<BrowseAccountingGroups, IPagedResult<AccountingGroupDto>> InstantiateHandler()
            => new BrowseAccountingGroupsHandler(Repository, Mapper);
    }
}
