using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.RestEase;
using Barista.StockItems.Domain;
using Barista.StockItems.Dto;
using Barista.StockItems.Repositories;
using Barista.StockItems.Services;
using Barista.StockItems.Verifiers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.StockItems
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

            services.AddScoped<IStockItemRepository, StockItemRepository>();
            services.AddDbContext<StockItemsDbContext>(opts => opts.UseMySql(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddCommonBaristaServices(Configuration);

            services.RegisterServiceForwarder<IPointsOfSaleService>("points-of-sale-service");
            services.AddTransient<IPointOfSaleVerifier, PointOfSaleVerifier>();

            services.AddAutoMapper(cfg => cfg.CreateMap<StockItem, StockItemDto>());
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
            app.UseAutomaticMigration<StockItemsDbContext>();
        }
    }
}
