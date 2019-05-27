using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.AccountingGroup;
using Barista.Api.Commands.Operations;
using Barista.Api.Dto;
using Barista.Api.Models.AccountingGroups;
using Barista.Api.Queries;
using Barista.Api.ResourceAuthorization;
using Barista.Api.ResourceAuthorization.Policies;
using Barista.Api.Services;
using Barista.Common;
using Barista.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [Route("api/accountingGroups")]
    [ApiController]
    public class AccountingGroupsController : BaseController
    {
        private readonly IAccountingGroupsService _accountingGroupsService;
        private readonly IAccountingGroupAuthorizationLoader _authLoader;

        public AccountingGroupsController(IBusPublisher busPublisher, IAccountingGroupsService accountingGroupsService, IAccountingGroupAuthorizationLoader authLoader) : base(busPublisher)
        {
            _accountingGroupsService = accountingGroupsService ?? throw new ArgumentNullException(nameof(accountingGroupsService));
            _authLoader = authLoader ?? throw new ArgumentNullException(nameof(authLoader));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IPagedResult<AccountingGroup>>> BrowseAccountingGroups([FromQuery] DisplayNameQuery query) =>
            Collection(await _accountingGroupsService.BrowseAccountingGroups(query));

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(AccountingGroup))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AccountingGroup>> GetAccountingGroup(Guid id) =>
            Single(await _accountingGroupsService.GetAccountingGroup(id));

        [Authorize]
        [HttpGet("{id}/authorizedUsers")]
        [ProducesResponseType(200, Type = typeof(IPagedResult<AccountingGroupAuthorizedUser>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IPagedResult<AccountingGroupAuthorizedUser>>> BrowseAuthorizedUsers(Guid id, [FromQuery] BrowseAccountingGroupAuthorizedUsers query) =>
            Collection(await _accountingGroupsService.BrowseAuthorizedUsers(id, query));

        [Authorize]
        [HttpGet("{id}/authorizedUsers/{userId}")]
        [ProducesResponseType(200, Type = typeof(AccountingGroupAuthorizedUser))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AccountingGroupAuthorizedUser>> GetAuthorizedUser(Guid id, Guid userId) =>
            Single(await _accountingGroupsService.GetAuthorizedUser(id, userId));

        [Authorize(Policies.IsUser)]
        [HttpPost("{id}/authorizedUsers/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> CreateAuthorizedUser(Guid id, Guid userId, UserAuthorizationDto userAuthorization)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);

            return await SendAndHandleOperationCommand(
                new CreateAccountingGroupUserAuthorization(id, userId, userAuthorization.Level)
            );
        }

        [Authorize(Policies.IsUser)]
        [HttpDelete("{id}/authorizedUsers/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteAuthorizedUser(Guid id, Guid userId)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);

            return await SendAndHandleOperationCommand(
                new DeleteAccountingGroupUserAuthorization(id, userId)
            );
        }

        [Authorize(Policies.IsUser)]
        [HttpPut("{id}/authorizedUsers/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateAuthorizedUser(Guid id, Guid userId, UserAuthorizationDto userAuthorization)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);

            return await SendAndHandleOperationCommand(
                new UpdateAccountingGroupUserAuthorization(id, userId, userAuthorization.Level)
            );
        }

        [HttpPost]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(202)]
        public async Task<ActionResult> CreateAccountingGroup(AccountingGroupDto accountingGroup)
        {
            var agId = Guid.NewGuid();

            return await StartLongRunningOperation(new HandleCreationOfAccountingGroup(
                agId, accountingGroup.DisplayName, accountingGroup.SaleStrategyId, User.GetUserId()
            ), agId);
        }

        [HttpPut("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(202)]
        public async Task<ActionResult> UpdateAccountingGroup(Guid id, AccountingGroupDto accountingGroup)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsAuthorizedUserPolicy.Instance);

            return await SendAndHandleOperationCommand(
                new UpdateAccountingGroup(id, accountingGroup.DisplayName, accountingGroup.SaleStrategyId)
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAccountingGroup(Guid id)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);
            return await SendAndHandleOperationCommand(new DeleteAccountingGroup(id));
        }
    }
}