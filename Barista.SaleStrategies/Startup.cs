using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.RestEase;
using Barista.SaleStrategies.Domain;
using Barista.SaleStrategies.Dto;
using Barista.SaleStrategies.Repositories;
using Barista.SaleStrategies.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.SaleStrategies
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
            services.AddScoped<ISaleStrategyRepository, SaleStrategyRepository>();
            services.AddCommonBaristaServices(Configuration);
            services.RegisterServiceForwarder<IAccountingService>("accounting-service");
            services.AddAutoMapper(cfg => cfg.CreateMap<SaleStrategy, SaleStrategyDto>());
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
