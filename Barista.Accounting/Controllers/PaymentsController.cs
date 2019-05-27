using System;
using System.Threading.Tasks;
using Barista.Accounting.Dto;
using Barista.Accounting.Queries;
using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Accounting.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : BaristaController
    {
        public PaymentsController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<PaymentDto>> BrowsePayments([FromQuery] BrowsePayments query)
        {
            return await QueryAsync(query);
        }

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPayment(Guid id)
        {
            var payment = await QueryAsync(new GetPayment{Id = id});
            if (payment is null)
                return NotFound();

            return payment;
        }       
    }
}