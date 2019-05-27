using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.Swipe;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Swipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [ApiController]
    [Route("api/swipe")]
    public class SwipeController : BaseController
    {
        public SwipeController(IBusPublisher busPublisher) : base(busPublisher)
        {
        }

        [HttpPost("start")]
        [Authorize(Policies.IsPointOfSale)]
        public async Task<IActionResult> ProcessSwipe(ProcessSwipe command)
        {
            if (User.IsPointOfSale())
                command = command.Bind(cmd => cmd.PointOfSaleId, User.GetPointOfSaleId());

            var result = await SendRequest<IProcessSwipe, IIdentifierResult>(command);
            if (!result.Successful)
                return result.ToActionResult();

            return CreatedAtAction("GetSale", "Sales", new {id = result.Id.Value}, null);
        }

        [HttpPost("cancel")]
        [Authorize(Policies.IsPointOfSale)]
        public async Task<ActionResult<IOperationResult>> CancelSwipe(CancelSwipe command)
        {
            if (User.IsPointOfSale())
                command = command.Bind(cmd => cmd.PointOfSaleId, User.GetPointOfSaleId());

            return await SendAndHandleOperationCommand(command);
        }

        [HttpPost("confirm")]
        [Authorize(Policies.IsPointOfSale)]
        public async Task<ActionResult<IOperationResult>> ConfirmSwipe(ConfirmSwipe command)
        {
            if (User.IsPointOfSale())
                command = command.Bind(cmd => cmd.PointOfSaleId, User.GetPointOfSaleId());

            return await SendAndHandleOperationCommand(command);
        }
    }
}
