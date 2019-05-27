using System;
using Barista.Contracts.Events.AccountingGroup;

namespace Barista.AccountingGroups.Events.AccountingGroup
{
    public class AccountingGroupDeleted : IAccountingGroupDeleted
    {
        public AccountingGroupDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
