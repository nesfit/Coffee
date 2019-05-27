using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.SpendingLimits;
using Barista.Api.Dto;
using Barista.Api.Models.Identity;
using Barista.Common;
using Barista.Common.Dto;
using Barista.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    public partial class AssignmentsToUserController
    {
        [HttpGet("{userAssignmentId}/spendingLimits")]
        [ProducesResponseType(200, Type = typeof(ResultPage<SpendingLimit>))]
        [Authorize(Policies.IsUser)]
        public async Task<ActionResult<IPagedResult<SpendingLimit>>> BrowseSpendingLimits(Guid userAssignmentId, [FromQuery] PagedQuery query)
        {
            var assignment = await _identityService.GetAssignmentToUser(userAssignmentId);
            if (assignment is null)
                return NotFound();

            if (assignment.AssignedToUserId != User.GetUserId() && !(await _authService.IsAdministrator(User)))
                return Unauthorized(new
                {
                    message = $"Only administrators can view other users' spending limits.",
                    code = "unauthorized_resource_access"
                });

            return await _identityService.BrowseSpendingLimits(userAssignmentId, query);
        }

        [HttpGet("{userAssignmentId}/spendingLimits/{spendingLimitId}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200, Type = typeof(SpendingLimit))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SpendingLimit>> GetSpendingLimit(Guid userAssignmentId, Guid spendingLimitId)
        {
            var assignment = await _identityService.GetAssignmentToUser(userAssignmentId);
            if (assignment is null)
                return NotFound();

            if (assignment.AssignedToUserId != User.GetUserId() && !(await _authService.IsAdministrator(User)))
                return Unauthorized(new
                {
                    message = $"Only administrators can view other users' spending limits.",
                    code = "unauthorized_resource_access"
                });

            return await _identityService.GetSpendingLimit(userAssignmentId, spendingLimitId);
        }

        [HttpPost("{userAssignmentId}/spendingLimits")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [Authorize(Policies.IsAdministrator)]
        public async Task<ActionResult> CreateSpendingLimit(Guid userAssignmentId, SpendingLimitDto spendingLimitDto)
            => await SendAndHandleIdentifierResultCommand(
                new CreateSpendingLimit(Guid.NewGuid(), userAssignmentId, spendingLimitDto.Interval, spendingLimitDto.Value),
                nameof(GetSpendingLimit),
                spendingLimitId => new { userAssignmentId, spendingLimitId }
            );

        [HttpPut("{userAssignmentId}/spendingLimits/{spendingLimitId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Policies.IsAdministrator)]
        public async Task<ActionResult<SpendingLimit>> UpdateSpendingLimit(Guid userAssignmentId, Guid spendingLimitId, SpendingLimitDto spendingLimitDto)
            => await SendAndHandleOperationCommand(
                new UpdateSpendingLimit(spendingLimitId, userAssignmentId, spendingLimitDto.Interval, spendingLimitDto.Value)
            );

        [HttpDelete("{userAssignmentId}/spendingLimits/{spendingLimitId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Policies.IsAdministrator)]
        public async Task<ActionResult<SpendingLimit>> DeleteSpendingLimit(Guid userAssignmentId, Guid spendingLimitId)
            => await SendAndHandleOperationCommand(new DeleteSpendingLimit(spendingLimitId, userAssignmentId));
    }
}
