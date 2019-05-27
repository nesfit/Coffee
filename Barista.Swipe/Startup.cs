using Barista.Common.AspNetCore;
using Barista.Common.RestEase;
using Barista.Swipe.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.Swipe
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
            services.RegisterServiceForwarder<IAccountingService>("accounting-service");
            services.RegisterServiceForwarder<IAccountingGroupsService>("accounting-groups-service");
            services.RegisterServiceForwarder<IIdentityService>("identity-service");
            services.RegisterServiceForwarder<IOffersService>("offers-service");
            services.RegisterServiceForwarder<IPointsOfSaleService>("points-of-sale-service");
            services.RegisterServiceForwarder<IProductsService>("products-service");
            services.RegisterServiceForwarder<ISaleStrategiesService>("sale-strategies-service");
            services.RegisterServiceForwarder<IUsersService>("users-service");
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
