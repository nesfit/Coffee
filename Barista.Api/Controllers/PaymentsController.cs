using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.Payment;
using Barista.Api.Dto;
using Barista.Api.Models.Accounting;
using Barista.Api.Queries;
using Barista.Api.Services;
using Barista.Common;
using Barista.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : BaseController
    {
        private readonly IAccountingService _accountingService;
        private readonly IAuthorizationService _authService;

        public PaymentsController(IBusPublisher busPublisher, IAccountingService accountingService, IAuthorizationService authService) : base(busPublisher)
        {
            _accountingService = accountingService ?? throw new ArgumentNullException(nameof(accountingService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpGet]
        [Authorize(Policies.IsAdministrator)]
        public async Task<ActionResult<IPagedResult<Payment>>> BrowsePayments([FromQuery] BrowsePayments query)
            => Collection(await _accountingService.BrowsePayments(query));

        [HttpGet("me")]
        [Authorize(Policies.IsUser)]
        public async Task<ActionResult<IPagedResult<Payment>>> BrowseOwnPayments([FromQuery] BrowsePayments query)
            => Collection(await _accountingService.BrowsePayments(query.Bind(q => q.CreditedToUser, User.GetUserId())));


        [HttpGet("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(Payment))]
        public async Task<ActionResult<Payment>> GetPayment(Guid id)
        {
            var payment = await _accountingService.GetPayment(id);
            
            if (payment is null)
                return NotFound();

            if (payment.UserId != User.GetUserId() && !(await _authService.IsAdministrator(User)))
                return Unauthorized(new
                {
                    message = $"Only administrators can view other users' payments.",
                    code = "unauthorized_resource_access"
                });

            return payment;
        }

        [HttpPost]
        [Authorize(Policies.CreatePayments)]
        public async Task<ActionResult> CreatePayment(PaymentDto paymentDto)
        {
            return await SendAndHandleIdentifierResultCommand(
                new CreatePayment(Guid.NewGuid(), paymentDto.Amount, paymentDto.UserId, paymentDto.Source, paymentDto.ExternalId),
                nameof(GetPayment)
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.DeletePayments)]
        public async Task<ActionResult> DeletePayment(Guid id)
        {
            return await SendAndHandleOperationCommand(new DeletePayment(id));
        }

        [HttpPut("{id}")]
        [Authorize(Policies.UpdatePayments)]
        public async Task<ActionResult> UpdatePayment(Guid id, UpdatePayment command)
        {
            return await SendAndHandleOperationCommand(command.Bind(cmd => cmd.Id, id));
        }
    }
}