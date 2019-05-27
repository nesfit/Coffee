using System;
using System.Collections.Generic;
using System.Linq;
using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.RestEase;
using Barista.PointsOfSale.Domain;
using Barista.PointsOfSale.Dto;
using Barista.PointsOfSale.Repositories;
using Barista.PointsOfSale.Services;
using Barista.PointsOfSale.Verifiers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.PointsOfSale
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

            services.AddDbContext<PointsOfSaleDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddScoped<IPointOfSaleRepository, PointOfSaleRepository>();
            services.AddScoped<IUserAuthorizationRepository, UserAuthorizationRepository>();

            services.AddCommonBaristaServices(Configuration);
            services.RegisterServiceForwarder<IAccountingGroupsService>("accounting-groups-service");
            services.RegisterServiceForwarder<ISaleStrategiesService>("sale-strategies-service");
            services.AddTransient<IAccountingGroupVerifier, AccountingGroupVerifier>();
            services.AddTransient<ISaleStrategyVerifier, SaleStrategyVerifier>();

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<PointOfSale, PointOfSaleDto>().ForMember(
                    posDto => posDto.KeyValues,
                    memberCfg => memberCfg.MapFrom(pos => pos.KeyValues.ToDictionary(kv =>kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase))
                );

                cfg.CreateMap<UserAuthorization, UserAuthorizationDto>();
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
            app.UseAutomaticMigration<PointsOfSaleDbContext>();
        }
    }
}
