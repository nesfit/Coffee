using System;
using System.Linq;
using System.Reflection;
using Barista.Common.AspNetCore;
using Barista.Common.RestEase;
using Barista.Mqtt.MessageHandlers;
using Barista.Mqtt.Services;
using Barista.Mqtt.TopicClassifiers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace Barista.Mqtt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opts => opts.Filters.Add<ValidateQueryParametersFilter>()).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCommonBaristaServices(Configuration);
            services.AddSingleton(Configuration);
            services.AddHostedService<MqttListenerService>();
            services.AddSingleton<IManagedMqttClientOptions>(sp =>
            {
                return new ManagedMqttClientOptionsBuilder()
                    .WithClientOptions(
                        new MqttClientOptionsBuilder()
                            .WithTcpServer(Configuration.GetConnectionString("MqttHost"),
                                int.Parse(Configuration.GetConnectionString("MqttPort")))
                            .WithCredentials(Configuration.GetConnectionString("MqttUser"),
                                Configuration.GetConnectionString("MqttPass"))
                            .WithClientId($"{nameof(MqttListenerService)}_{sp.GetRequiredService<IServiceId>().Id}")
                            .WithCleanSession()
                            .Build()
                    )
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .Build();
            });

            services.AddSingleton<IManagedMqttClient>(new MqttFactory().CreateManagedMqttClient());
            services.AddSingleton<IPosTopicClassifier, PosTopicClassifier>();
            services.AddSingleton<IPosFeatureVerifier, PosFeatureVerifier>();
            services.AddSingleton<IMqttMessagePublisher, MqttMessagePublisher>();
            services.RegisterServiceForwarder<IPointsOfSaleService>("points-of-sale-service");
            services.RegisterServiceForwarder<IOffersService>("offers-service");

            foreach (var messageHandlerType in Assembly.GetEntryAssembly().DefinedTypes.Where(t => t.IsClass && !t.IsAbstract && typeof(IMqttMessageHandler).IsAssignableFrom(t)))
                services.AddSingleton(typeof(IMqttMessageHandler), messageHandlerType);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCommonBaristaServices();
            app.UseMvc();
        }
    }
}
