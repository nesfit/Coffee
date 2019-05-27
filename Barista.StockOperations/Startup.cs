using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.RestEase;
using Barista.StockOperations.Domain;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Repositories;
using Barista.StockOperations.Services;
using Barista.StockOperations.Verifiers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.StockOperations
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
            services.AddDbContext<StockOperationsDbContext>(opts => opts.UseMySql(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddScoped<IStockOperationRepository, StockOperationRepository>();
            services.AddCommonBaristaServices(Configuration);

            services.RegisterServiceForwarder<IAccountingService>("accounting-service");
            services.RegisterServiceForwarder<IStockItemsService>("stock-items-service");
            services.RegisterServiceForwarder<IUsersService>("users-service");
            services.AddTransient<ISaleVerifier, SaleVerifier>();
            services.AddTransient<IStockItemVerifier, StockItemVerifier>();
            services.AddTransient<IUserVerifier, UserVerifier>();

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<StockOperation, StockOperationDto>();
                cfg.CreateMap<ManualStockOperation, ManualStockOperationDto>();
                cfg.CreateMap<SaleBasedStockOperation, SaleBasedStockOperationDto>();
            });
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
            app.UseAutomaticMigration<StockOperationsDbContext>();
        }
    }
}
