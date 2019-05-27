using System;
using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.MassTransit;
using Barista.Contracts.Commands.Operations;
using Barista.Operations.Activities.AccountingGroup.Create;
using Barista.Operations.Repository;
using Barista.Operations.Tracking;
using Barista.Operations.TrackingService;
using GreenPipes;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration.Saga;
using MassTransit.ExtensionsDependencyInjectionIntegration.Configuration.Registration;
using MassTransit.RabbitMqTransport;
using MassTransit.Saga;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.Operations
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
            services.AddDbContext<OperationsDbContext>(dbOptionsB => dbOptionsB.UseMySql(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddCommonBaristaServices(Configuration, (host,configurator,sp) => ConfigureMassTransit(services, sp, host, configurator));
        }

        private void ConfigureMassTransit(IServiceCollection services, IServiceProvider serviceProvider, IRabbitMqHost host, IRabbitMqBusFactoryConfigurator cfg)
        {
             var machine = new RoutingSlipStateMachine();

             services.AddSingleton<ISagaRepository<RoutingSlipState>>(sp => new EntityFrameworkSagaRepository<RoutingSlipState>(() => sp.GetRequiredService<OperationsDbContext>(), optimistic: true));
            
            cfg.ReceiveEndpoint(host, "routing_slip_state", e =>
            {
                e.PrefetchCount = 8;
                e.UseConcurrencyLimit(1);
                e.UseRetry(x =>
                {
                    x.Handle<DbUpdateConcurrencyException>();
                    x.Interval(5, TimeSpan.FromMilliseconds(100));
                }); // Add the retry middleware for optimistic concurrency
                e.StateMachineSaga(machine, new EntityFrameworkSagaRepository<RoutingSlipState>(() => new OperationsDbContext(new DbContextOptionsBuilder().UseMySql(Configuration.GetConnectionString("DatabaseConnection")).Options), optimistic:true));
            });

            cfg.ConfigureActivities(host, serviceProvider, Configuration);
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
            app.UseAutomaticMigration<OperationsDbContext>();
        }
    }
}
