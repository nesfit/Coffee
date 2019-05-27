using System;
using Barista.Contracts.Commands.AccountingGroupUserAuthorization;

namespace Barista.Operations.Commands.AccountingGroupUserAuthorization
{
    public class CreateAccountingGroupOwnership : ICreateAccountingGroupUserAuthorization
    {
        public Guid AccountingGroupId { get; }
        public Guid UserId { get; }
        public string Level => "Owner";

        public CreateAccountingGroupOwnership(Guid accountingGroupId, Guid userId)
        {
            AccountingGroupId = accountingGroupId;
            UserId = userId;
        }
    }
}
