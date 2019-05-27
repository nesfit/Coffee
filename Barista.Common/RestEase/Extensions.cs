using System;
using System.Linq;
using System.Net.Http;
using Barista.Common.Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase;

namespace Barista.Common.RestEase
{
    // (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public static class Extensions
    {
        public static void RegisterServiceForwarder<T>(this IServiceCollection services, string serviceName)
            where T : class
        {
            var clientName = typeof(T).ToString();
            var options = GetRestEaseOptions(services);
            var serviceOptions = options.Services?.SingleOrDefault(s => s.Name == serviceName);

            switch (options.LoadBalancer)
            {
                case "consul":
                    ConfigureConsulClient(services, clientName, serviceName);
                    break;

                case "fabio":
                    ConfigureFabioClient(services, clientName);
                    break;
                
                default:
                    ConfigureDefaultClient(services, clientName, serviceName, serviceOptions);
                    break;
            }
           
            ConfigureForwarder<T>(services, clientName);
        }

        private static RestEaseOptions GetRestEaseOptions(IServiceCollection services)
        {
            return GetConfiguration(services).GetSection("restEase").Get<RestEaseOptions>();
        }

        private static IFabioOptions GetFabioOptions(IServiceCollection services)
        {
            return GetConfiguration(services).GetSection("fabio").Get<FabioOptions>();
        }

        private static IConfiguration GetConfiguration(IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
                configuration = serviceProvider.GetService<IConfiguration>();

            return configuration;
        }

        private static void ConfigureConsulClient(IServiceCollection services, string clientName, string serviceName)
        {
            services.AddHttpClient(clientName)
                .AddHttpMessageHandler(c =>
                    new ConsulServiceDiscoveryMessageHandler(c.GetService<IConsulServicesRegistry>(),
                        c.GetService<ConsulOptions>(), serviceName, overrideRequestUri: true));
        }

        private static bool _fabioInitialized = false;

        private static void ConfigureFabioClient(IServiceCollection services, string clientName)
        {
            if (!_fabioInitialized)
            {
                services.AddSingleton<IFabioOptions>(GetFabioOptions(services));
                services.AddTransient<FabioMessageHandler>();
                _fabioInitialized = true;
            }

            services.AddHttpClient(clientName, cfg =>
                {
                    cfg.BaseAddress = new Uri(GetFabioOptions(services).Url);
                })
                .AddHttpMessageHandler<FabioMessageHandler>();
        }

        private static void ConfigureDefaultClient(IServiceCollection services, string clientName,
            string serviceName, RestEaseOptions.Service service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            services.AddHttpClient(clientName, client =>
            {
                client.BaseAddress = new UriBuilder
                {
                    Scheme = service.Scheme,
                    Host = service.Host,
                    Port = service.Port
                }.Uri;
            });
        }

        private static void ConfigureForwarder<T>(IServiceCollection services, string clientName) where T : class
        {
            services.AddTransient<T>(c => new RestClient(c.GetService<IHttpClientFactory>().CreateClient(clientName))
            {
                ResponseDeserializer = new ResponseDeserializer(),
                RequestQueryParamSerializer = new QueryParamSerializer()
            }.For<T>());
        }
    }
}
