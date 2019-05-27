using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Barista.Api.ResourceAuthorization
{
    public class ResourceAuthorizationFailedExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ResourceAuthorizationFailedExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ResourceAuthorizationFailedException e)
            {
                var response = new
                {
                    message = $"This operation on resource {e.ResourceName} with ID '{e.ResourceIdentifier}' requires access level of {e.RequiredLevel}",
                    code = "unauthorized_resource_access"
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
