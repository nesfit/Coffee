using System;
using Barista.Contracts.Commands.AccountingGroupUserAuthorization;

namespace Barista.Api.Commands.AccountingGroup
{
    public class DeleteAccountingGroupUserAuthorization : IDeleteAccountingGroupUserAuthorization
    {
        public DeleteAccountingGroupUserAuthorization(Guid accountingGroupId, Guid userId)
        {
            AccountingGroupId = accountingGroupId;
            UserId = userId;
        }

        public Guid AccountingGroupId { get; }
        public Guid UserId { get; }
    }
}
