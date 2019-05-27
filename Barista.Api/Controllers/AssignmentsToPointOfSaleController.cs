using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.AssignmentToPointOfSale;
using Barista.Api.Dto;
using Barista.Api.Models.Identity;
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
    [ApiController]
    [Route("api/assignmentsToPointOfSale")]
    public class AssignmentsToPointOfSaleController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly IPointOfSaleAuthorizationLoader _posAuthLoader;

        public AssignmentsToPointOfSaleController(IBusPublisher busPublisher, IIdentityService identityService, IPointOfSaleAuthorizationLoader posAuthLoader) : base(busPublisher)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _posAuthLoader = posAuthLoader ?? throw new ArgumentNullException(nameof(posAuthLoader));
        }

        [HttpGet]
        [Authorize(Policies.BrowseAssignmentsToPointOfSale)]
        public async Task<ActionResult<IPagedResult<AssignmentToPointOfSale>>> BrowseAssignmentsToPointOfSale([FromQuery] BrowseAssignmentsToPointOfSale query)
            => Collection(await _identityService.BrowseAssignmentsToPointOfSale(query));

        [HttpGet("pointOfSale/{posId}")]
        [Authorize(Policies.IsUser)]
        public async Task<ActionResult<IPagedResult<AssignmentToPointOfSale>>> BrowseAssignmentsToSpecificPointOfSale(Guid posId, [FromQuery] BrowseAssignmentsToPointOfSale query)
        {
            await _posAuthLoader.AssertResourceAccessAsync(User, posId, IsAuthorizedUserPolicy.Instance);
            return Collection(await _identityService.BrowseAssignmentsToPointOfSale(query));
        }

        [HttpGet("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200, Type = typeof(AssignmentToPointOfSale))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AssignmentToPointOfSale>> GetAssignmentToPointOfSale(Guid id)
        {
            var assignment = await _identityService.GetAssignmentToPointOfSale(id);
            if (assignment is null)
                return NotFound();

            await _posAuthLoader.AssertResourceAccessAsync(User, assignment.AssignedToPointOfSaleId, IsAuthorizedUserPolicy.Instance);
            return assignment;
        }

        [HttpPost]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> CreateAssignmentToPointOfSale(AssignmentToPointOfSaleDto assignmentDto)
        {
            await _posAuthLoader.AssertResourceAccessAsync(User, assignmentDto.PointOfSaleId, IsAuthorizedUserPolicy.Instance);

            var command = new CreateAssignmentToPointOfSale(Guid.NewGuid(), assignmentDto.MeansId, assignmentDto.ValidSince, assignmentDto.ValidUntil, assignmentDto.PointOfSaleId);
            return await SendAndHandleIdentifierResultCommand(command, nameof(GetAssignmentToPointOfSale));
        }

        [HttpPut("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateAssignmentToPointOfSale(Guid id, AssignmentToPointOfSale assignmentDto)
        {
            var assignment = await _identityService.GetAssignmentToPointOfSale(id);
            if (assignment is null)
                return NotFound();

            await _posAuthLoader.AssertResourceAccessAsync(User, assignment.AssignedToPointOfSaleId, IsAuthorizedUserPolicy.Instance);

            if (assignmentDto.AssignedToPointOfSaleId != assignment.AssignedToPointOfSaleId)
                await _posAuthLoader.AssertResourceAccessAsync(User, assignment.AssignedToPointOfSaleId, IsAuthorizedUserPolicy.Instance);

            return await SendAndHandleOperationCommand(
                new UpdateAssignmentToPointOfSale(id, assignmentDto.AssignedToPointOfSaleId, assignmentDto.ValidSince, assignment.ValidUntil, assignment.AssignedToPointOfSaleId)
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteAssignmentToPointOfSale(Guid id)
        {
            var assignment = await _identityService.GetAssignmentToPointOfSale(id);
            if (assignment is null)
                return NotFound();

            await _posAuthLoader.AssertResourceAccessAsync(User, assignment.AssignedToPointOfSaleId, IsAuthorizedUserPolicy.Instance);
            return await SendAndHandleOperationCommand(new DeleteAssignmentToPointOfSale(id));
        }
    }
}
