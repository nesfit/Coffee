using System.Threading.Tasks;
using Barista.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace Barista.Api.Authorization
{
    public class UnderlyingMeansExistsRequirementHandler : AuthorizationHandler<UnderlyingMeansExistsRequirement>
    {
        private readonly IIdentityService _idService;

        public UnderlyingMeansExistsRequirementHandler(IIdentityService idService)
        {
            _idService = idService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UnderlyingMeansExistsRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == Claims.MeansId))
            {
                context.Fail();
                return;
            }

            var meansId = context.User.GetMeansId();
            var httpResponse = await _idService.StatAuthenticationMeans(meansId);
            if (httpResponse.IsSuccessStatusCode)
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}
