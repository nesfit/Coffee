using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GreenPipes;
using GreenPipes.Internals.Extensions;
using MassTransit;
using MassTransit.Courier;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Barista.Common.MassTransit
{
    public static class Extensions
    {
        public static ICorrelationContext ToCorrelationContext(this ConsumeContext ctx)
        {
            return new CorrelationContext(ctx.MessageId.GetValueOrDefault(), ctx.ConversationId.GetValueOrDefault(), ctx.CorrelationId.GetValueOrDefault());
        }

        public static T Cast<T>(object what) => (T) what;

        private static Assembly EntryAssembly => Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Could not obtain entry assembly");

        private static IEnumerable<(Type CommandHandlerType, Type CommandType, Type ResultType)> DefinedCommandHandlers
        {
            get
            {
                foreach (var commandHandlerType in EntryAssembly.DefinedTypes
                    .Where(t => t.HasInterface(typeof(ICommandHandler<,>))))
                {
                    var genericArguments = commandHandlerType.GetInterface(typeof(ICommandHandler<,>))
                        .GetGenericArguments();

                    var commandType = genericArguments[0];
                    if (!commandType.IsInterface)
                        continue;

                    var resultType = genericArguments[1];

                    yield return (commandHandlerType, commandType, resultType);
                }
            }
        }

        private static IEnumerable<(Type EventHandlerType, Type EventType)> DefinedEventHandlers
        {
            get
            {
                foreach (var eventHandlerType in EntryAssembly.DefinedTypes
                    .Where(t => t.HasInterface(typeof(IEventHandler<>))))
                {
                    var genericArguments = eventHandlerType.GetInterface(typeof(IEventHandler<>)).GetGenericArguments();

                    var eventType = genericArguments[0];
                    if (!eventType.IsInterface)
                        continue;

                    yield return (eventHandlerType, eventType);
                }
            }
        }

        private static IBusControl BusFactory(IServiceProvider serviceProvider, Action<IRabbitMqHost, IRabbitMqBusFactoryConfigurator, IServiceProvider> configurator)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();

            var hostname = configuration.GetConnectionString("RabbitMQHost");
            var username = configuration.GetConnectionString("RabbitMQUser");
            var password = configuration.GetConnectionString("RabbitMQPassword");
            var vhost = configuration.GetConnectionString("RabbitMQVhost");

            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(hostname, vhost, hostCfg =>
                {
                    hostCfg.Username(username);
                    hostCfg.Password(password);
                });

                cfg.UseExtensionsLogging(serviceProvider.GetRequiredService<ILoggerFactory>());

                if (DefinedActivities.Any())
                    cfg.ReceiveEndpoint(host, Assembly.GetEntryAssembly().GenerateRoutingSlipFailureQueueName(), x =>
                    {
                        x.PrefetchCount = 10;
                        x.UseMessageRetry(rCfg => rCfg.Interval(5, TimeSpan.FromSeconds(30)));
                        x.Consumer(typeof(RoutingSlipFailureConsumer), serviceProvider.GetRequiredService);                        
                    });

                foreach (var definedCommandHandler in DefinedCommandHandlers)
                {
                    var (_, commandType, resultType) = definedCommandHandler;

                    string queueName;
                    const string usualCommandsNamespace = "Barista.Contracts.Commands.";
                    if (commandType.FullName.StartsWith(usualCommandsNamespace))
                        queueName = commandType.UnderscorizePascalCamelCase(usualCommandsNamespace);
                    else
                        throw new BaristaException("unknown_message_namespace", $"The command '{commandType.FullName}' is not assigned to any message namespace");

                    void Configure(IRabbitMqReceiveEndpointConfigurator epCfg)
                    {
                        epCfg.PrefetchCount = 1;
                        epCfg.UseMessageRetry(rCfg => rCfg.Interval(5, TimeSpan.FromSeconds(30)));
                        epCfg.Consumer(typeof(CommandHandlerWrapper<,>).MakeGenericType(commandType, resultType), serviceProvider.GetRequiredService);
                    }

                    cfg.ReceiveEndpoint(
                        host,
                        queueName,
                        Configure);

                    System.Diagnostics.Debug.WriteLine($"Configured endpoint for qn: \"{queueName}\"");
                }

                foreach (var definedEventHandler in DefinedEventHandlers)
                {
                    var (_, eventType) = definedEventHandler;

                    string queueName;
                    const string usualEventsNamespace = "Barista.Contracts.Events.";
                    if (eventType.FullName.StartsWith(usualEventsNamespace))
                        queueName = eventType.UnderscorizePascalCamelCase(usualEventsNamespace);
                    else
                        throw new BaristaException("unknown_message_namespace", $"The event '{eventType.FullName}' is not assigned to any message namespace");

                    void Configure(IRabbitMqReceiveEndpointConfigurator epCfg)
                    {
                        epCfg.PrefetchCount = 1;
                        epCfg.UseMessageRetry(rCfg => rCfg.Interval(5, TimeSpan.FromSeconds(30)));
                        epCfg.Consumer(typeof(EventHandlerWrapper<>).MakeGenericType(eventType), serviceProvider.GetRequiredService);
                    }

                    cfg.ReceiveEndpoint(
                        host,
                        queueName + "_" + EntryAssembly.GetName().Name,
                        Configure
                    );
                }

                configurator?.Invoke(host, cfg, serviceProvider);
            });
        }

        public static void AddMassTransit(this IServiceCollection serviceCollection, Action<IRabbitMqHost, IRabbitMqBusFactoryConfigurator, IServiceProvider> configurator = null, params Type[] activitiesToAdd)
        {
            foreach (var definedCommandHandler in DefinedCommandHandlers)
                serviceCollection.AddScoped(typeof(ICommandHandler<,>).MakeGenericType(definedCommandHandler.CommandType, definedCommandHandler.ResultType),
                    definedCommandHandler.CommandHandlerType);

            foreach (var definedEventHandler in DefinedEventHandlers)
                serviceCollection.AddScoped(typeof(IEventHandler<>).MakeGenericType(definedEventHandler.EventType),
                    definedEventHandler.EventHandlerType);

            foreach (var definedActivityType in DefinedActivities)
                serviceCollection.AddScoped(definedActivityType);

            if (DefinedActivities.Any())
                serviceCollection.AddSingleton<RoutingSlipFailureConsumer>();

            serviceCollection.AddSingleton<IHostedService, BusService>();
            serviceCollection.AddSingleton<IBusPublisher, BusPublisher>();
            serviceCollection.AddSingleton(typeof(CommandHandlerWrapper<,>), typeof(CommandHandlerWrapper<,>));
            serviceCollection.AddSingleton(typeof(EventHandlerWrapper<>), typeof(EventHandlerWrapper<>));
            serviceCollection.AddScoped<IRoutingSlipTransactionBuilderFactory, RoutingSlipTransactionBuilderFactory>();

            serviceCollection.AddMassTransit(x => x.AddBus(sp => BusFactory(sp, configurator)));
        }

        public static string GetActivityExchangeName(this Type activityType)
        {
            string exchangeName;
            const string usualCommandsNamespace = "Barista.";
            if (activityType.FullName.StartsWith(usualCommandsNamespace))
                exchangeName = activityType.UnderscorizePascalCamelCase(usualCommandsNamespace);
            else
                throw new BaristaException("unknown_activity_namespace", $"The activity '{activityType.FullName}' resides in unsupported namespace");

            return exchangeName;
        }

        public static string GetCompensatingActivityExchangeName(this Type activityType) => GetActivityExchangeName(activityType) + ".compensations";

        public static Uri AppendActivityExchangeName(this Uri busUri, Type activityType)
        {
            return new Uri(busUri, GetActivityExchangeName(activityType));
        }

        public static Uri AppendCompensatingActivityExchangeName(this Uri busUri, Type activityType)
        {
            return new Uri(busUri, GetCompensatingActivityExchangeName(activityType));
        }

        public static string GenerateRoutingSlipFailureQueueName(this Assembly assembly)
        {
            return assembly.GetName().Name + "_RoutingSlipFailures";
        }

        public static IRabbitMqBusFactoryConfigurator ConfigureCompensatingActivity<TCompensateActivity, TCompensateActivityParameters, TCompensateActivityLog>(this IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host, IServiceProvider serviceProvider, string busUriStr)
        where TCompensateActivity : class, Activity<TCompensateActivityParameters, TCompensateActivityLog>
        where TCompensateActivityParameters : class
        where TCompensateActivityLog : class
        {
            cfg.ReceiveEndpoint(host, typeof(TCompensateActivity).GetCompensatingActivityExchangeName(), cc =>
            {
                cc.PrefetchCount = 100;
                cc.UseMessageRetry(rCfg => rCfg.Interval(5, TimeSpan.FromSeconds(30)));
                cc.CompensateActivityHost<TCompensateActivity, TCompensateActivityLog>(serviceProvider);
            });

            return cfg;
        }

        public static IRabbitMqBusFactoryConfigurator ConfigureExecuteActivity<TExecuteActivity, TExecuteActivityParameters>(this IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host, IServiceProvider serviceProvider, string busUriStr)
            where TExecuteActivity : class, ExecuteActivity<TExecuteActivityParameters>
            where TExecuteActivityParameters : class
        {
            cfg.ReceiveEndpoint(host, typeof(TExecuteActivity).GetActivityExchangeName(), cc =>
            {
                cc.PrefetchCount = 100;
                cc.UseMessageRetry(rCfg => rCfg.Interval(5, TimeSpan.FromSeconds(30)));

                if (typeof(ICompensateActivity).IsAssignableFrom(typeof(TExecuteActivity)))
                {
                    cc.ExecuteActivityHost<TExecuteActivity, TExecuteActivityParameters>(new Uri(busUriStr + typeof(TExecuteActivity).GetCompensatingActivityExchangeName()), serviceProvider);
                }
                else
                    cc.ExecuteActivityHost<TExecuteActivity, TExecuteActivityParameters>(serviceProvider);
            });

            return cfg;
        }

        private static IEnumerable<Type> DefinedActivities =>
            EntryAssembly.DefinedTypes.Where(t =>
                !t.IsAbstract && t.IsClass && (typeof(IActivity).IsAssignableFrom(t) ||
                                               typeof(IExecuteActivity).IsAssignableFrom(t)));

        public static IRabbitMqBusFactoryConfigurator ConfigureActivities(this IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host, IServiceProvider serviceProvider, IConfiguration svcCfg)
        {
            var busUriStr = new Uri(new Uri("rabbitmq://" + svcCfg.GetConnectionString("RabbitMQHost") + "/"), svcCfg.GetConnectionString("RabbitMQVhost")).ToString().TrimEnd('/') + '/';
            var invocationParameters = new object[] {cfg, host, serviceProvider, busUriStr};
            
            foreach (var activityType in DefinedActivities)
            {
                var interfaces = activityType.GetInterfaces();
                var argumentsType = interfaces.Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ExecuteActivity<>)).GenericTypeArguments.Single();

                if (typeof(ICompensateActivity).IsAssignableFrom(activityType))
                {
                    var logType = interfaces.Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(CompensateActivity<>)).GenericTypeArguments.Single();

                    typeof(Extensions)
                        .GetMethod(nameof(ConfigureCompensatingActivity))
                        .MakeGenericMethod(activityType, argumentsType, logType)
                        .Invoke(null, invocationParameters);
                }

                if (typeof(IExecuteActivity).IsAssignableFrom(activityType))
                {
                    typeof(Extensions)
                        .GetMethod(nameof(ConfigureExecuteActivity))
                        .MakeGenericMethod(activityType, argumentsType)
                        .Invoke(null, invocationParameters);
                }
            }

            return cfg;
        }
    }
}
