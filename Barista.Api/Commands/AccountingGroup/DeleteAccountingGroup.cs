using System;
using Barista.Contracts.Commands.AccountingGroup;

namespace Barista.Api.Commands.AccountingGroup
{
    public class DeleteAccountingGroup : IDeleteAccountingGroup
    {
        public DeleteAccountingGroup(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
