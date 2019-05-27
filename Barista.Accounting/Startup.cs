using System.Linq;
using Barista.Accounting.Repositories;
using Barista.Accounting.Services;
using Barista.Accounting.Verifiers;
using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.RestEase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.Accounting
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
            services.AddDbContext<AccountingDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddScoped<IPaymentsRepository, PaymentsRepository>();
            services.AddScoped<ISalesRepository, SalesRepository>();
            services.AddScoped<IBalanceRepository, BalanceRepository>();

            services.RegisterServiceForwarder<IAccountingGroupsService>("accounting-groups-service");
            services.RegisterServiceForwarder<IIdentityService>("identity-service");
            services.RegisterServiceForwarder<IPointsOfSaleService>("points-of-sale-service");
            services.RegisterServiceForwarder<IUsersService>("users-service");
            services.RegisterServiceForwarder<IProductsService>("products-service");
            services.RegisterServiceForwarder<IOffersService>("offers-service");

            services.AddTransient<IAccountingGroupVerifier, AccountingGroupVerifier>();
            services.AddTransient<IAuthenticationMeansVerifier, AuthenticationMeansVerifier>();
            services.AddTransient<IPointOfSaleVerifier, PointOfSaleVerifier>();
            services.AddTransient<IUserVerifier, UserVerifier>();
            services.AddTransient<IProductVerifier, ProductVerifier>();
            services.AddTransient<IOfferVerifier, OfferVerifier>();
            services.AddTransient<ISpendingRepository, SpendingRepository>();

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<Domain.Payment, Dto.PaymentDto>();
                cfg.CreateMap<Domain.Sale, Dto.SaleDto>().ForMember(s => s.StateChanges, opts => opts.MapFrom(s => s.StateChanges.ToArray()));
                cfg.CreateMap<Domain.SaleStateChange, Dto.SaleStateChangeDto>();
                cfg.CreateMap<Domain.Balance, Dto.BalanceDto>();
                cfg.CreateMap<Domain.SpendingOfUsers, Dto.SpendingOfUserDto>();
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
            app.UseAutomaticMigration<AccountingDbContext>();
            app.UseMvc();
        }
    }
}
