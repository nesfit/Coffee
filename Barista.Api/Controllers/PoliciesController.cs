using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Barista.Api.Controllers
{
    [ApiController]
    [Route("api/policies")]
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyListProvider _policyListProvider;
        private readonly IAuthorizationService _authorization;
        private readonly ILogger<PoliciesController> _logger;

        public PoliciesController(IPolicyListProvider policyListProvider, IAuthorizationService authorization, ILogger<PoliciesController> logger)
        {
            _policyListProvider = policyListProvider ?? throw new ArgumentNullException(nameof(policyListProvider));
            _authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<string>>> GetOwnFulfilledPolicies()
        {
            var policyTasks = _policyListProvider.GetAll().ToDictionary(
                policyEntry => policyEntry.Key,
                policyEntry => Task.Run(() => _authorization.AuthorizeAsync(User, policyEntry.Value))
            );

            await Task.WhenAll(policyTasks.Values);

            var policyKeys = policyTasks.Where(policyEntry => !policyEntry.Value.IsFaulted && policyEntry.Value.Result.Succeeded)
                .Select(policyEntry => policyEntry.Key);

            return Ok(policyKeys);
        }
    }
}
