using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.AssignmentToUser;
using Barista.Api.Dto;
using Barista.Api.Models.Identity;
using Barista.Api.Queries;
using Barista.Api.Services;
using Barista.Common;
using Barista.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [ApiController]
    [Route("api/assignmentsToUser")]
    public partial class AssignmentsToUserController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly IAuthorizationService _authService;

        public AssignmentsToUserController(IBusPublisher busPublisher, IIdentityService identityService, IAuthorizationService authService) : base(busPublisher)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpGet]
        [Authorize(Policies.BrowseAssignmentsToUser)]
        [ProducesResponseType(200, Type = typeof(IPagedResult<AssignmentToUser>))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IPagedResult<AssignmentToUser>>> BrowseAssignmentsToUser([FromQuery] BrowseAssignmentsToUser query)
            => Collection(await _identityService.BrowseAssignmentsToUser(query));

        [HttpGet("me")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200, Type = typeof(IPagedResult<AssignmentToUser>))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IPagedResult<AssignmentToUser>>> BrowseOwnAssignments([FromQuery] BrowseAssignmentsToUser query)
            => Collection(await _identityService.BrowseAssignmentsToUser(query.Bind(q => q.AssignedToUser, User.GetUserId())));

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(AssignmentToUser))]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AssignmentToUser>> GetAssignmentToUser(Guid id)
        {
            var assignment = await _identityService.GetAssignmentToUser(id);
            if (assignment is null)
                return NotFound();

            if (assignment.AssignedToUserId != User.GetUserId() && !(await _authService.AuthorizeAsync(User, Policies.IsAdministrator)).Succeeded)
                return Unauthorized(new
                {
                    message = $"Only administrators can view other users' assignments.",
                    code = "unauthorized_resource_access"
                });

            return assignment;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [Authorize(Policies.IsUser)]
        public async Task<ActionResult> CreateAssignmentToUser(AssignmentToUserDto dto)
        {
            if (!(await _authService.IsAdministrator(User)))
            {
                if (dto.UserId != Guid.Empty)
                {
                    ModelState.AddModelError("field_is_readonly", "Only administrators may create assignments to user for other users");
                    return ValidationProblem();
                }

                dto.UserId = User.GetUserId();
            }

            return await SendAndHandleIdentifierResultCommand(
                new CreateAssignmentToUser(Guid.NewGuid(), dto.MeansId, dto.ValidSince, dto.ValidUntil, dto.UserId, dto.IsShared),
                nameof(GetAssignmentToUser)
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Policies.IsUser)]
        public async Task<ActionResult> UpdateAssignmentToUser(Guid id, AssignmentToUserDto dto)
        {
            if (!(await _authService.IsAdministrator(User)))
            {
                if (dto.UserId != Guid.Empty)
                {
                    ModelState.AddModelError("field_is_readonly", "Only administrators may re-assign assignments to user to other users");
                    return ValidationProblem();
                }

                dto.UserId = User.GetUserId();
            }

            return await SendAndHandleOperationCommand(
                new UpdateAssignmentToUser(id, dto.MeansId, dto.ValidSince, dto.ValidUntil, dto.UserId, dto.IsShared)
            );
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Authorize(Policies.IsUser)]
        public async Task<ActionResult> DeleteAssignmentToUser(Guid id)
        {
            var assignment = await _identityService.GetAssignmentToUser(id);
            if (assignment is null)
                return NotFound();

            if (assignment.AssignedToUserId != User.GetUserId() && !(await _authService.IsAdministrator(User)))
                return Unauthorized(new
                {
                    message = $"Only administrators can delete other users' assignments to user.",
                    code = "unauthorized_resource_access"
                });

            return await SendAndHandleOperationCommand(new DeleteAssignmentToUser(id));
        }
    }
}
