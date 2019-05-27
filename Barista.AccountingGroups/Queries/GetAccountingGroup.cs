using System;
using Barista.AccountingGroups.Dto;
using Barista.Contracts;

namespace Barista.AccountingGroups.Queries
{
    public class GetAccountingGroup : IQuery<AccountingGroupDto>
    {
        public GetAccountingGroup(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
