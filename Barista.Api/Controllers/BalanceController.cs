using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Models.Accounting;
using Barista.Api.Services;
using Barista.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [Route("api/balance")]
    [ApiController]
    public class BalanceController : BaseController
    {
        private readonly IAccountingService _accountingService;

        public BalanceController(IBusPublisher busPublisher, IAccountingService accountingService) : base(busPublisher)
        {
            _accountingService = accountingService ?? throw new ArgumentNullException(nameof(accountingService));
        }

        [HttpGet("me")]
        [Authorize(Policies.IsUser)]
        public async Task<ActionResult<UserBalance>> GetCurrentUserBalance()
            => Single(await _accountingService.GetBalance(User.GetUserId()));

        [HttpGet("{userId}")]
        [Authorize(Policies.IsAdministrator)]
        public async Task<ActionResult<UserBalance>> GetUserBalance(Guid userId)
            => Single(await _accountingService.GetBalance(userId));
    }
}