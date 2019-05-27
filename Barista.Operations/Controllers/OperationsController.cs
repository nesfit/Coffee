using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Operations.Commands.AccountingGroup;
using Barista.Operations.Dto;
using Barista.Operations.Repository;
using Barista.Operations.Tracking;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Operations.Controllers
{
    [Route("api/operations")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly OperationsDbContext _dbContext;

        public OperationsController(OperationsDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OperationDto>> GetOperation(Guid id)
        {
            var operation = await _dbContext.RoutingSlipStates.FindAsync(id);
            if (operation is null)
                return NotFound();

            return new OperationDto(operation.CorrelationId, operation.EndTime.HasValue, operation.FaultSummary != null, operation.Duration);
        }
    }
}