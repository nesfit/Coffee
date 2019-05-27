using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestEase;

namespace Barista.Common.AspNetCore
{
    public class BaristaExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BaristaExceptionHandlingMiddleware> _logger;

        public BaristaExceptionHandlingMiddleware(RequestDelegate next, ILogger<BaristaExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException restEaseException)
            {
                _logger.LogWarning(restEaseException, "Error receiving data from downstream service");

                var response = new
                {
                    message = "downstream_service_unavailable",
                    code = $"Endpoint is currently unavailable (returned status code {restEaseException.StatusCode})."
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 502;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
            catch (BaristaException e)
            {
                var code = e.Code;
                _logger.LogInformation(e, $"An exception with code {code} was handled by the middleware into a HTTP {e.RecommendedStatusCode ?? HttpStatusCode.InternalServerError} result");

                var response = new
                {
                    message = e.Message,
                    code = e.Code
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) (e.RecommendedStatusCode ?? HttpStatusCode.InternalServerError);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
