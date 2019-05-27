using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Dto;
using Barista.AccountingGroups.Queries;
using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Barista.AccountingGroups.Controllers
{
    [Route("api/accountingGroups")]
    [ApiController]
    public class AccountingGroupsController : BaristaController
    {
        public AccountingGroupsController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<AccountingGroupDto>> BrowseAccountingGroups([FromQuery] BrowseAccountingGroups query)
        {
            return await QueryAsync(query);
        }

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<AccountingGroupDto>> GetAccountingGroup(Guid id)
        {
            var ag = await QueryAsync(new GetAccountingGroup(id));
            if (ag is null)
                return NotFound();

            return ag;
        }

        [HttpGet("{accountingGroupId}/authorizedUsers")]
        public async Task<IPagedResult<UserAuthorizationDto>> BrowseAuthorizedUsers(Guid accountingGroupId, [FromQuery] BrowseUserAuthorizations query)
        {
            query.Bind(q => q.AccountingGroupId, accountingGroupId);
            return await QueryAsync(query);
        }

        [HttpGet("{accountingGroupId}/authorizedUsers/{userId}")]
        [HttpHead("{accountingGroupId}/authorizedUsers/{userId}")]
        public async Task<ActionResult<UserAuthorizationDto>> GetUserAuthorization(Guid accountingGroupId, Guid userId)
        {
            var userAUth = await QueryAsync(new GetUserAuthorization(accountingGroupId, userId));
            if (userAUth is null)
                return NotFound();

            return userAUth;
        }

        [HttpHead("authorizedUsers/{userId}")]
        public async Task<IActionResult> FindAuthorizedUser(Guid userId)
        {
            var result = await QueryAsync(new BrowseUserAuthorizations {UserId = userId, ResultsPerPage = 1});
            return result.TotalResults > 0 ? (IActionResult)Ok() : NoContent();
        }

        [HttpGet("userAuthorizations/{userId}")]
        public async Task<IPagedResult<UserAuthorizationDto>> BrowseUserAuthorizations(Guid userId, [FromQuery] BrowseUserAuthorizations query)
            => await QueryAsync(query.Bind(q => q.UserId, userId));
    }
}
