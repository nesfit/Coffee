using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using Barista.Contracts;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.SwaggerGeneration.Processors.Security;

namespace Barista.Common
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommandHandlers(this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null)
                throw new ArgumentNullException(nameof(serviceCollection));

            foreach (var handler in Assembly.GetEntryAssembly().DefinedTypes.Where(t =>
                t.GetInterfaces().Any(tt =>
                    tt.IsGenericType && tt.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))))
            {
                var serviceType = handler.GetInterfaces().Single(tt => tt.IsGenericType && tt.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));
                var impl = handler;

                serviceCollection.AddTransient(serviceType, handler);
            }
        }

        public static void AddQueryHandlers(this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null)
                throw new ArgumentNullException(nameof(serviceCollection));

            foreach (var handler in Assembly.GetEntryAssembly().DefinedTypes.Where(t => t.GetInterfaces().Any(tt => tt.IsGenericType && tt.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))))
                serviceCollection.AddScoped(handler.GetInterfaces().Single(tt => tt.IsGenericType && tt.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)), handler);
        }

        public static void AddEventHandlers(this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null)
                throw new ArgumentNullException(nameof(serviceCollection));

            foreach (var handler in Assembly.GetEntryAssembly().DefinedTypes.Where(t => t.GetInterfaces().Any(tt => tt.IsGenericType && tt.GetGenericTypeDefinition() == typeof(IEventHandler<>))))
                serviceCollection.AddScoped(handler.GetInterfaces().Single(tt => tt.IsGenericType && tt.GetGenericTypeDefinition() == typeof(IEventHandler<>)), handler);
        }

        public static void AddBaristaSwagger(this IServiceCollection serviceCollection, string title, string description, string version = "v1")
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddSwaggerDocument(d => d.DocumentName = $"{title} {version}");
        }

        public static void AddAutoMapper(this IServiceCollection serviceCollection, Action<IMapperConfigurationExpression> mapperCfg = null)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap(typeof(IPagedResult<>), typeof(IPagedResult<>)).ConvertUsing(typeof(PagedResultConverter<,>));
                mapperCfg?.Invoke(cfg);
            });

            Mapper.AssertConfigurationIsValid();
            serviceCollection.AddSingleton<IMapper>(Mapper.Instance);
        }
    }
}
