using System;
using Barista.Common.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Barista.Common
{
    public static class ApplicationBuilderExtensions 
    {
        public static void UseBaristaSwagger(this IApplicationBuilder appBuilder, string serviceName)
        {
            if (appBuilder == null) throw new ArgumentNullException(nameof(appBuilder));
            appBuilder.UseSwagger();
            appBuilder.UseSwaggerUi3();
        }

        public static IApplicationBuilder UseBaristaExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<BaristaExceptionHandlingMiddleware>();

        public static IApplicationBuilder UseAutomaticMigration<TDbContext>(this IApplicationBuilder appBuilder) where TDbContext : DbContext
        {
            using (var serviceScope = appBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var lifetime = serviceScope.ServiceProvider.GetRequiredService<IApplicationLifetime>();
                var loggerFactory = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("AutomaticDatabaseMigration");

                do {
                    try {
                        logger.LogInformation("Establishing database connection to carry out automatic migration, if applicable.");
                        TDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();

                        logger.LogInformation("Database context established, applying applicable migrations.");
                        DatabaseFacade dbFacade = (DatabaseFacade) ((dynamic) dbContext.Database);
                        dbFacade.Migrate();

                        logger.LogInformation("Database migration was not required or was just performed.");
                    }
                    catch (Exception e) {
                        logger.LogCritical(e, "Failed to establish database connection and/or carry out automatic migration of database context. Retrying in 5 seconds.");
                        System.Threading.Tasks.Task.Delay(5000, lifetime.ApplicationStopping).GetAwaiter().GetResult();
                        continue;
                    }
                } while (false);
            }

            return appBuilder;
        }
    }
}
