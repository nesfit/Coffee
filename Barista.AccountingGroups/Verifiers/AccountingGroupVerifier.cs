using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Repositories;
using Barista.Common;

namespace Barista.AccountingGroups.Verifiers
{
    public class AccountingGroupVerifier : IAccountingGroupVerifier
    {
        private readonly IAccountingGroupRepository _repository;

        public AccountingGroupVerifier(IAccountingGroupRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task AssertExists(Guid entityId)
        {
            if ((await _repository.GetAsync(entityId)) is null)
                throw new BaristaException("accounting_group_not_found", $"Could not find accounting group with ID '{entityId}'");
        }
    }
}
