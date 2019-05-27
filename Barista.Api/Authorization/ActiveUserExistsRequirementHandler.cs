using System;
using System.Threading.Tasks;
using Barista.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace Barista.Api.Authorization
{
    public class ActiveUserExistsRequirementHandler : AuthorizationHandler<ActiveUserExistsRequirement>
    {
        private readonly IUsersService _usersService;

        public ActiveUserExistsRequirementHandler(IUsersService usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveUserExistsRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == Claims.UserId))
                return;

            var userId = context.User.GetUserId();
            var user = await _usersService.GetUser(userId);
            if (user?.IsActive == true)
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}
