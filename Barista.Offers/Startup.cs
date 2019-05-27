using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.RestEase;
using Barista.Offers.Domain;
using Barista.Offers.Dto;
using Barista.Offers.Repositories;
using Barista.Offers.Services;
using Barista.Offers.Verifiers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.Offers
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
            services.AddDbContext<OffersDbContext>(opts => opts.UseMySql(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddCommonBaristaServices(Configuration);

            services.RegisterServiceForwarder<IPointsOfSaleService>("points-of-sale-service");
            services.RegisterServiceForwarder<IProductsService>("products-service");
            services.RegisterServiceForwarder<IStockItemsService>("stock-items-service");
            services.AddTransient<IPointOfSaleVerifier, PointOfSaleVerifier>();
            services.AddTransient<IProductVerifier, ProductVerifier>();
            services.AddTransient<IStockItemVerifier, StockItemVerifier>();

            services.AddAutoMapper(cfg => { cfg.CreateMap<Offer, OfferDto>(); });
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
            app.UseAutomaticMigration<OffersDbContext>();
        }
    }
}
