using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Operations.Commands.AccountingGroup;
using Barista.Operations.Tracking;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Operations.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IBusPublisher _busPublisher;
        private readonly IBusControl _busControl;

        public TestController(IBusPublisher busPublisher, IBusControl busControl)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _busControl = busControl ?? throw new ArgumentNullException(nameof(busControl));
        }

        [HttpGet]
        public async Task<ActionResult> Honk()
        {
            var result =
                await _busPublisher.SendRequest<CreateAccountingGroup, IIdentifierResult>(
                    new CreateAccountingGroup(Guid.NewGuid(), "ControllerTest", Guid.Empty));

            if (result.Successful)
                return new OkObjectResult(result.Id.Value);
            else
                return BadRequest(result.ErrorCode);
        }

        [HttpPost]
        public async Task<ActionResult> Test()
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            builder.AddActivity("CreateGroup", new Uri("rabbitmq://localhost/vhost/operations.activities.accountinggroup.create_accounting_group"));
            builder.AddActivity("AssignOwnership", new Uri("rabbitmq://localhost/vhost/operations.activities.accountinggroup.assign_accounting_group_ownership"));

            builder.SetVariables(new
            {
                Id = Guid.NewGuid(),
                DisplayName = "Test",
                SaleStrategyId = Guid.Parse("{DDF6FE32-F6C7-42DA-0001-4FD368D0D8B9}"),
                OwnerUserId = Guid.Parse("{DDF6FE32-F6C7-42DA-0002-4FD368D0D8B9}")
            });

            RoutingSlip routingSlip = builder.Build();

            await _busControl.Publish<RoutingSlipCreated>(new
            {
                TrackingNumber = routingSlip.TrackingNumber,
                Timestamp = routingSlip.CreateTimestamp,
            });

            await _busControl.Execute(routingSlip);
            return new OkObjectResult(routingSlip.TrackingNumber);
        }


    }
}