using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Identity.Controllers
{
    [ApiController]
    [Route("api/assignmentsToUser")]
    public class AssignmentsToUserController : BaristaController
    {
        public AssignmentsToUserController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<AssignmentToUserDto>> BrowseAssignments([FromQuery] BrowseAssignmentsToUser query)
        {
            return await QueryAsync(query);
        }

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<AssignmentToUserDto>> GetAssignment(Guid id)
        {
            var assignment = await QueryAsync(new GetAssignmentToUser(id));
            if (assignment is null)
                return NotFound();

            return assignment;
        }

        [HttpGet("{id}/spendingLimits")]
        public async Task<IPagedResult<SpendingLimitDto>> BrowseSpendingLimits(Guid id, [FromQuery] BrowseSpendingLimits browseLimits)
        {
            return await QueryAsync(browseLimits.Bind(q => q.ParentAssignmentToUserId, id));
        }

        [HttpGet("{id}/spendingLimits/{spendingLimitId}")]
        [HttpHead("{id}/spendingLimits/{spendingLimitId}")]
        public async Task<ActionResult<SpendingLimitDto>> GetSpendingLimit(Guid id, Guid spendingLimitId)
        {
            var spendingLimit = await QueryAsync(new GetSpendingLimit(spendingLimitId, id));
            if (spendingLimit is null)
                return NotFound();

            return spendingLimit;
        }
    }
}
