using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Barista.Api.Authorization
{
    public class JwtFromCookieLoadingMiddleware
    {
        public const string JwtCookieName = "jwt";
        private const string HeaderName = "Authorization";
        private static readonly Regex Base64CharacterSetValidator = new Regex(@"^[A-Za-z0-9\+\=_\-\.]+$");

        private readonly RequestDelegate _next;

        public JwtFromCookieLoadingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private void AppendAuthorizationHeaderWithJwt(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(HeaderName))
                return; // The client already provided their own authorization header.

            if (!context.Request.Cookies.TryGetValue(JwtCookieName, out var jwt))
                return; // There's no cookie to extract the JWT from.

            if (!Base64CharacterSetValidator.IsMatch(jwt))
                return; // The JWT token contains disallowed characters.

            context.Request.Headers.Append("Authorization", "Bearer " + jwt);
        }

        public async Task Invoke(HttpContext context)
        {
            AppendAuthorizationHeaderWithJwt(context);
            await _next.Invoke(context);
        }
    }
}
