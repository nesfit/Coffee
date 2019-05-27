using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Barista.Common.AspNetCore;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Common.Consul
{
    // (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public static class Extensions
    {
        private const string ConsulSectionName = "consul";

        public static IServiceCollection AddConsul(this IServiceCollection services)
        {
            services.AddTransient(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var consulOptions = new ConsulOptions();

                var section = configuration.GetSection(ConsulSectionName);
                configuration.Bind(ConsulSectionName, consulOptions);
                return consulOptions;
            });

            ConsulOptions options;
            using (var provider = services.BuildServiceProvider())
                options = provider.GetService<ConsulOptions>();

            services.AddTransient<IConsulServicesRegistry, ConsulServicesRegistry>();
            services.AddTransient<ConsulServiceDiscoveryMessageHandler>();

            return services.AddSingleton<IConsulClient>(c => new ConsulClient(cfg =>
            {
                if (!string.IsNullOrEmpty(options.Url))
                {
                    cfg.Address = new Uri(options.Url);
                }
            }));
        }

        //Returns unique service ID used for removing the service from registry.
        public static void UseConsul(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var consulOptions = scope.ServiceProvider.GetService<ConsulOptions>();
                var enabled = consulOptions.Enabled;
                var consulEnabled = Environment.GetEnvironmentVariable("CONSUL_ENABLED")?.ToLowerInvariant();

                if (!string.IsNullOrWhiteSpace(consulEnabled))
                {
                    enabled = consulEnabled == "true" || consulEnabled == "1";
                }

                if (!enabled)
                {
                    return;
                }


                var address = consulOptions.Address;
                if (string.IsNullOrWhiteSpace(address))
                {
                    throw new ArgumentException("Consul address can not be empty.",
                        nameof(consulOptions.PingEndpoint));
                }

                var uniqueId = scope.ServiceProvider.GetService<IServiceId>().Id;
                var client = scope.ServiceProvider.GetService<IConsulClient>();
                var serviceName = consulOptions.Service;
                var serviceId = $"{serviceName}:{uniqueId}";
                var port = consulOptions.Port;
                var pingEndpoint = consulOptions.PingEndpoint;
                var pingInterval = consulOptions.PingInterval <= 0 ? 5 : consulOptions.PingInterval;
                var removeAfterInterval =
                    consulOptions.RemoveAfterInterval <= 0 ? 10 : consulOptions.RemoveAfterInterval;

                IEnumerable<string> urlPrefixes;
                if (consulOptions.UrlPrefixes != null)
                {
                    urlPrefixes = consulOptions.UrlPrefixes;
                }
                else
                    urlPrefixes = Assembly.GetEntryAssembly().DefinedTypes
                        .Where(t => t.IsClass && typeof(BaristaController).IsAssignableFrom(t))
                        .ToDictionary(t => t, t => t.GetCustomAttribute(typeof(RouteAttribute)) as RouteAttribute)
                        .Where(kvp => kvp.Value?.Template?.StartsWith("api/") == true)
                        .Select(kvp => kvp.Value.Template);

                var registration = new AgentServiceRegistration
                {
                    Name = serviceName,
                    ID = serviceId,
                    Address = address,
                    Port = port,
                    Tags = urlPrefixes.Select(urlPrefix => $"urlprefix-/{urlPrefix}").ToArray()
                };

                Console.WriteLine("Registration prefixes:");
                foreach (var tag in registration.Tags)
                    Console.WriteLine($"- {tag}");

                if (consulOptions.PingEnabled)
                {
                    var scheme = address.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)
                        ? string.Empty
                        : "http://";

                    var check = new AgentServiceCheck
                    {
                        Interval = TimeSpan.FromSeconds(pingInterval),
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(removeAfterInterval),
                        HTTP = $"{scheme}{address}{(port > 0 ? $":{port}" : string.Empty)}/{pingEndpoint}"
                    };

                    registration.Checks = new[] {check};
                }

                var lifetime = scope.ServiceProvider.GetService<IApplicationLifetime>();
                lifetime.ApplicationStarted.Register(() => client.Agent.ServiceRegister(registration));            
                lifetime.ApplicationStopped.Register(() => client.Agent.ServiceDeregister(serviceId));
            }
        }
    }
}