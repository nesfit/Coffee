using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.MassTransit;
using Barista.Common.RestEase;
using Barista.Consistency.Repositories;
using Barista.Consistency.Services;
using Barista.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.Consistency
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
            services.AddCommonBaristaServices(Configuration, (rmqHost, rmqBfCfg, sp) => rmqBfCfg.ConfigureActivities(rmqHost, sp, Configuration));
            services.AddDbContext<ConsistencyDbContext>(opts => opts.UseMySql(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddScoped<IScheduledEventRepository, ScheduledEventRepository>();
            services.AddScoped<IEventDispatcher, EventDispatcher>();

            var consistencySection = Configuration.GetSection("Consistency").Get<ConsistencyConfiguration>();
            services.AddSingleton<IConsistencyConfiguration>(consistencySection);

            services.RegisterServiceForwarder<IAccountingGroupsService>("accounting-groups-service");
            services.RegisterServiceForwarder<IAccountingService>("accounting-service");
            services.RegisterServiceForwarder<IIdentityService>("identity-service");
            services.RegisterServiceForwarder<IOffersService>("offers-service");
            services.RegisterServiceForwarder<IPointsOfSaleService>("points-of-sale-service");
            services.RegisterServiceForwarder<IStockOperationsService>("stock-operations-service");

            services.AddHostedService<ScheduledEventDispatcherService>();
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
            app.UseAutomaticMigration<ConsistencyDbContext>();
        }
    }
}
