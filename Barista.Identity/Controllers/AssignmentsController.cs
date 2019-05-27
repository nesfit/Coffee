using System;
using System.Threading.Tasks;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Identity.Controllers
{
    [Route("api/assignments")]
    [ApiController]
    public class AssignmentsController : BaristaController
    {
        public AssignmentsController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<AssignmentDto>> BrowseAssignments([FromQuery] BrowseAssignments query)
        {
            return await QueryAsync(query);
        }

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<AssignmentDto>> GetAssignment(Guid id)
        {
            var assignment = await QueryAsync(new GetAssignment(id));
            if (assignment is null)
                return NotFound();

            return assignment;
        }
    }
}
