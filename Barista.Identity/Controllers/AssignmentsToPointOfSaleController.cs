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
    [ApiController]
    [Route("api/assignmentsToPointOfSale")]
    public class AssignmentsToPointOfSaleController : BaristaController
    {
        public AssignmentsToPointOfSaleController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<AssignmentToPointOfSaleDto>> BrowseAssignments([FromQuery] BrowseAssignmentsToPointOfSale query)
            => await QueryAsync(query);

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<AssignmentToPointOfSaleDto>> GetAssignment(Guid id)
        {
            var query = new GetAssignmentToPointOfSale(id);
            var assignmentToPos = await QueryAsync(query);
            if (assignmentToPos is null)
                return NotFound();

            return assignmentToPos;
        }
    }
}
