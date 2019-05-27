using System;
using System.Threading.Tasks;
using Barista.Api.Services;

namespace Barista.Api.ResourceAuthorization.Loaders
{
    public class AccountingGroupAuthorizationLoader : IAccountingGroupAuthorizationLoader
    {
        private readonly IAccountingGroupsService _accountingGroupsService;

        public AccountingGroupAuthorizationLoader(IAccountingGroupsService accountingGroupsService)
        {
            _accountingGroupsService = accountingGroupsService ?? throw new ArgumentNullException(nameof(accountingGroupsService));
        }

        public string ResourceName => "Accounting group";

        public async Task<IUserAuthorizationLevel> LoadUserAuthorizationLevel(Guid userId, Guid accountingGroupId)
        {
            return await _accountingGroupsService.GetAuthorizedUser(accountingGroupId, userId);
        }
    }
}
