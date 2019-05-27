using System;
using System.Net;
using System.Threading;
using Barista.Common.Consul;
using Barista.Common.Dispatchers;
using Barista.Common.MassTransit;
using Consul;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HealthStatus = Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus;

namespace Barista.Common.AspNetCore
{
    public static class Extensions
    {

        public static IServiceCollection AddCommonBaristaServices(this IServiceCollection serviceCollection, IConfiguration configuration, Action<IRabbitMqHost, IRabbitMqBusFactoryConfigurator, IServiceProvider> cfgAction = null)
        {
            serviceCollection.AddTransient<IServiceId, ServiceId>();
            serviceCollection.AddLogging(loggingBuilder =>  loggingBuilder.AddSeq(configuration.GetSection("Seq")));
            serviceCollection.AddCommandHandlers();
            serviceCollection.AddQueryHandlers();
            serviceCollection.AddEventHandlers();
            serviceCollection.AddMassTransit(cfgAction);
            serviceCollection.AddDispatchers();
            serviceCollection.AddHealthChecks();
            serviceCollection.AddConsul();
            serviceCollection.AddSingleton(typeof(IClientsidePaginator<>), typeof(ClientsidePaginator<>));
            return serviceCollection;
        }

        public static IApplicationBuilder UseCommonBaristaServices(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseBaristaExceptionHandler();

            applicationBuilder.UseHealthChecks("/ping", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready"),
                ResponseWriter = async (httpContext, healthReport) =>
                {
                    if (healthReport.Status == HealthStatus.Healthy)
                        httpContext.Response.StatusCode = (int) HttpStatusCode.OK;
                    else
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadGateway;

                    await httpContext.Response.WriteAsync(healthReport.Status.ToString(), CancellationToken.None);
                }
            });

            applicationBuilder.UseConsul();
            return applicationBuilder;
        }
    }
}
