using System;
using System.Threading.Tasks;
using Barista.Api.Models.Operation;
using Barista.Api.Services;
using Barista.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [Route("api/operations")]
    [ApiController]
    [Authorize]
    public class OperationsController : BaseController
    {
        private readonly IOperationsService _operationsService;

        public OperationsController(IOperationsService operationsService, IBusPublisher busPublisher) : base(busPublisher)
        {
            _operationsService = operationsService ?? throw new ArgumentNullException(nameof(operationsService));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Operation>> GetOperation(Guid id)
            => Single(await _operationsService.GetOperation(id));
    }
}