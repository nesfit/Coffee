using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.RestEase;
using Barista.Identity.Dto;
using Barista.Identity.Repositories;
using Barista.Identity.Services;
using Barista.Identity.Verifiers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.Identity
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
            services.AddDbContext<IdentityDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddMvc(opts => opts.Filters.Add<ValidateQueryParametersFilter>()).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IAuthenticationMeansRepository, AuthenticationMeansRepository>();
            services.AddScoped<IAssignmentsRepository, AssignmentsRepository>();
            services.AddScoped<IAssignedMeansRepository, AssignedMeansRepository>();

            services.RegisterServiceForwarder<IPointsOfSaleService>("points-of-sale-service");
            services.RegisterServiceForwarder<IUsersService>("users-service");

            services.AddTransient<IPointOfSaleVerifier, PointOfSaleVerifier>();
            services.AddTransient<IUserVerifier, UserVerifier>();
            services.AddScoped<IAuthenticationMeansVerifier, AuthenticationMeansVerifier>();
            services.AddScoped<IAssignmentExclusivityVerifier, AssignmentExclusivityVerifier>();

            services.AddCommonBaristaServices(Configuration);

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<Domain.Assignment, AssignmentDto>()
                    .ForMember(dto => dto.Means, opt => opt.MapFrom(a => a.MeansId));

                cfg.CreateMap<Domain.AssignmentToPointOfSale, AssignmentToPointOfSaleDto>()
                    .ForMember(dto => dto.Means, opt => opt.MapFrom(a => a.MeansId))
                    .ForMember(dto => dto.AssignedToPointOfSaleId, opt => opt.MapFrom(a => a.PointOfSaleId));

                cfg.CreateMap<Domain.AssignmentToUser, AssignmentToUserDto>()
                    .ForMember(dto => dto.Means, opt => opt.MapFrom(a => a.MeansId))
                    .ForMember(dto => dto.AssignedToUserId, opt => opt.MapFrom(a => a.UserId));

                cfg.CreateMap<Domain.AuthenticationMeans, AuthenticationMeansDto>()
                    .ForMember(dto => dto.Method, opt => opt.MapFrom(m => m.Method));

                cfg.CreateMap<Domain.AuthenticationMeans, AuthenticationMeansWithValueDto>()
                    .ForMember(dto => dto.Method, opt => opt.MapFrom(m => m.Method));

                cfg.CreateMap<Domain.SpendingLimit, SpendingLimitDto>();
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
            app.UseAutomaticMigration<IdentityDbContext>();
        }
    }
}
