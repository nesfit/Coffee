using System;
using System.Linq;
using Barista.Api.Authenticators;
using Barista.Api.Authenticators.Impl;
using Barista.Api.Authorization;
using Barista.Api.Models.Identity;
using Barista.Api.Models.Users;
using Barista.Api.ResourceAuthorization;
using Barista.Api.ResourceAuthorization.Loaders;
using Barista.Api.Services;
using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.RestEase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.SwaggerGeneration.Processors.Security;

namespace Barista.Api
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
            services.AddSingleton<IAuthorizationHandler, AdvancedUserRequirementHandler>();
            services.AddSingleton<IAuthorizationHandler, UnderlyingMeansExistsRequirementHandler>();
            services.AddSingleton<IAuthorizationHandler, ActiveUserExistsRequirementHandler>();
            services.AddSingleton<IAuthorizationHandler, PosExistsRequirementHandler>();

            var jwtSigningKeyBytes = Convert.FromBase64String(Configuration["Jwt:SigningKey"]);
            var jwtSigningKey = new SymmetricSecurityKey(jwtSigningKeyBytes);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false; // TODO x.RequireHttpsMetadata
                    x.SaveToken = true;

                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = Configuration["Jwt:Audience"],
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtSigningKey
                    };
                });

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy(Policies.IsUser, builder => builder.RequireAuthenticatedUser().RequireClaim(Claims.UserId).RequireClaim(Claims.MeansId).AddRequirements(new UnderlyingMeansExistsRequirement(), new ActiveUserExistsRequirement()));
                cfg.AddPolicy(Policies.IsPointOfSale, builder => builder.RequireAuthenticatedUser().RequireClaim(Claims.PointOfSaleId).RequireClaim(Claims.MeansId).AddRequirements(new UnderlyingMeansExistsRequirement(), new PosExistsRequirement()));
                cfg.AddPolicy(Policies.IsAdministrator, builder => builder.Combine(cfg.GetPolicy(Policies.IsUser)).RequireClaim(Claims.IsAdministrator, true.ToString()));
                cfg.AddPolicy(Policies.IsAdvancedUser, builder => builder.Combine(cfg.GetPolicy(Policies.IsUser)).AddRequirements(new AdvancedUserRequirement()));

                cfg.AddPolicy(Policies.CreateAccountingGroups, cfg.GetPolicy(Policies.IsAdministrator));

                cfg.AddPolicy(Policies.CreatePayments, cfg.GetPolicy(Policies.IsAdministrator));
                cfg.AddPolicy(Policies.DeletePayments, cfg.GetPolicy(Policies.IsAdministrator));
                cfg.AddPolicy(Policies.UpdatePayments, cfg.GetPolicy(Policies.IsAdministrator));

                cfg.AddPolicy(Policies.CreateUsers, cfg.GetPolicy(Policies.IsAdministrator));
                cfg.AddPolicy(Policies.DeleteUsers, cfg.GetPolicy(Policies.IsAdministrator));
                cfg.AddPolicy(Policies.UpdateUsers, cfg.GetPolicy(Policies.IsAdministrator));

                cfg.AddPolicy(Policies.CreatePointsOfSale, cfg.GetPolicy(Policies.IsAdministrator));

                cfg.AddPolicy(Policies.BrowseUserSummaries, builder => builder.RequireAuthenticatedUser());
                cfg.AddPolicy(Policies.BrowseUserDetails, cfg.GetPolicy(Policies.IsAdministrator));

                cfg.AddPolicy(Policies.ViewUserSummary, builder => builder.RequireAuthenticatedUser());
                cfg.AddPolicy(Policies.ViewUserDetails, cfg.GetPolicy(Policies.IsAdministrator));

                cfg.AddPolicy(Policies.BrowseAssignmentsToPointOfSale, cfg.GetPolicy(Policies.IsAdministrator));
                cfg.AddPolicy(Policies.BrowseAssignmentsToUser, cfg.GetPolicy(Policies.IsAdministrator));

                cfg.AddPolicy(Policies.BrowseSpendingLimits, cfg.GetPolicy(Policies.IsAdministrator));
            });

            services.RegisterServiceForwarder<IAccountingService>("accounting-service");
            services.RegisterServiceForwarder<IAccountingGroupsService>("accounting-groups-service");
            services.RegisterServiceForwarder<IIdentityService>("identity-service");
            services.RegisterServiceForwarder<IOffersService>("offers-service");
            services.RegisterServiceForwarder<IPointsOfSaleService>("points-of-sale-service");
            services.RegisterServiceForwarder<IProductsService>("products-service");
            services.RegisterServiceForwarder<ISaleStrategiesService>("sale-strategies-service");
            services.RegisterServiceForwarder<IStockItemsService>("stock-items-service");
            services.RegisterServiceForwarder<IStockOperationsService>("stock-operations-service");
            services.RegisterServiceForwarder<IUsersService>("users-service");
            services.RegisterServiceForwarder<IOperationsService>("operations-service");
            services.AddCommonBaristaServices(Configuration);
            
            services.AddMvc(opts => opts.Filters.Add<ValidateQueryParametersFilter>()).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerDocument(document =>
            {
                document.DocumentName = "Barista API";
                document.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT"));
                document.AddSecurity("JWT", Enumerable.Empty<string>(), new SwaggerSecurityScheme()
                {
                    Type = SwaggerSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = SwaggerSecurityApiKeyLocation.Header,
                    Description = "Type into the text box: Bearer {your JWT token}."
                });
            });

            services.AddTransient<IPointOfSaleAuthorizationLoader, PointOfSaleAuthorizationLoader>();
            services.AddTransient<IAccountingGroupAuthorizationLoader, AccountingGroupAuthorizationLoader>();
            services.AddTransient<IStockItemAuthorizationLoader, StockItemAuthorizationLoader>();
            services.AddTransient<IPosAgAuthorizationLoader, PosAgAuthorizationLoader>();
            services.AddSingleton<IPasswordHasher<AuthenticationMeansWithValue>, PasswordHasher<AuthenticationMeansWithValue>>();
            services.AddSingleton<IPolicyListProvider, PolicyListProvider>();

            services.AddSingleton<IMeansValueHasher, MeansValueHasher>();
            services.AddSingleton<IPointOfSaleApiKeyAuthenticator, PointOfSaleApiKeyAuthenticator>();
            services.AddSingleton<IUserApiKeyAuthenticator, UserApiKeyAuthenticator>();
            services.AddSingleton<IUserPasswordAuthenticator, UserPasswordAuthenticator>();

            services.AddSingleton<IApiKeyGenerator, ApiKeyGenerator>();

            services.AddAutoMapper(cfg => { cfg.CreateMap<FullUser, User>(); });

            services.AddCors(opts => opts.AddDefaultPolicy(
                builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Operation")
                    .AllowCredentials()
                    .WithOrigins(Configuration["CorsOrigins"].Split(","))
            ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware(typeof(JwtFromCookieLoadingMiddleware));
            app.UseAuthentication();
            app.UseCors();
            app.UseMiddleware(typeof(ResourceAuthorizationFailedExceptionMiddleware));
            app.UseCommonBaristaServices();
            app.UseMvc();
            app.UseBaristaSwagger("API");
        }
    }
}
