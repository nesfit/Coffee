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
    [Route("api/assignedMeans")]
    [ApiController]
    public class AssignedMeansController : BaristaController
    {
        public AssignedMeansController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet("toUser/{userId}")]
        public async Task<IPagedResult<AuthenticationMeansWithValueDto>> BrowseMeansAssignedToUser(Guid userId, [FromQuery] BrowseAssignedMeans query)
            => await QueryAsync(new BrowseAssignedMeansToUser(query, userId));

        [HttpGet("toPointOfSale/{posId}")]
        public async Task<IPagedResult<AuthenticationMeansWithValueDto>> BrowseMeansAssignedToPointOfSale(Guid posId, [FromQuery] BrowseAssignedMeans query)
            => await QueryAsync(new BrowseAssignedMeansToPointOfSale(query, posId));
    }
}
