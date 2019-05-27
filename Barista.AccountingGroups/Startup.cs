using Barista.AccountingGroups.Dto;
using Barista.AccountingGroups.Repositories;
using Barista.AccountingGroups.Services;
using Barista.AccountingGroups.Verifiers;
using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.RestEase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.AccountingGroups
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
            services.AddDbContext<AccountingGroupsDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddScoped<IAccountingGroupRepository, AccountingGroupRepository>();
            services.AddScoped<IUserAuthorizationRepository, UserAuthorizationRepository>();
            services.AddCommonBaristaServices(Configuration);

            services.RegisterServiceForwarder<IUsersService>("users-service");
            services.RegisterServiceForwarder<ISaleStrategiesService>("sale-strategies-service");
            services.AddTransient<IUserVerifier, UserVerifier>();
            services.AddTransient<ISaleStrategyVerifier, SaleStrategyVerifier>();
            services.AddScoped<IAccountingGroupVerifier, AccountingGroupVerifier>();

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<Domain.AccountingGroup, AccountingGroupDto>();
                cfg.CreateMap<Domain.UserAuthorization, UserAuthorizationDto>();
                cfg.CreateMap<Domain.UserAuthorizationLevel, UserAuthorizationLevelDto>();
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
            app.UseAutomaticMigration<AccountingGroupsDbContext>();
        }
    }
}
