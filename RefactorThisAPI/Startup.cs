using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using RefactorThis.API;
using RefactorThis.API.Logging;
using RefactorThis.Application;
using RefactorThis.Persistence;
using RefactorThis.Persistence.Sqlite;
using Serilog;

namespace RefactorThisAPI
{
    public class Startup
    {
        private ILoggerFactory _sqlLoggerFactory;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            if (env.IsDevelopment())
            {
                _sqlLoggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddFilter((category, level) =>
                                category == DbLoggerCategory.Database.Command.Name
                                && level == LogLevel.Information)
                            .AddConsole()   // when running from command prompt
                            .AddDebug();    // when debugging from VS
                });
            }
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Log.Logger);

            ConfigureAuth(services);

            ConfigureDb(services);

            services.AddControllers(options =>
            {
                options.Filters.Add<UnhandledExceptionFilter>();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RefactorThisAPI", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token in the text input below",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                           new OpenApiSecurityScheme
                             {
                                 Reference = new OpenApiReference
                                 {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = "Bearer"
                                 }
                             },
                             new string[] {}
                     }
                 });
            });

            services.AddMediatR(typeof(GetProductRequest).Assembly);
            services.TryAddScoped<IProductRepository, ProductRepository>();
            services.TryAddScoped<IProductOptionRepository, ProductOptionRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSecurityHeaders(SecurityHeadersDefinitions.GetHeaderPolicyCollection(env.IsDevelopment()));

            app.UseMiddleware<RequestLogger>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RefactorThisAPI v1");
                }
                );
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public virtual void ConfigureAuth(IServiceCollection services)
        {
            // https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-aspnet-core-web-api
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddMicrosoftIdentityWebApi(Configuration, "AzureAd");

            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                // The claim in the Jwt token where App roles are available.
                options.TokenValidationParameters.RoleClaimType = "roles";
            });
        }

        public virtual void ConfigureDb(IServiceCollection services)
        {
            services.AddDbContext<RefactorThisDbContext>(options =>
            {
                options.UseSqlite("Data Source=../App_Data/products.db;");
                options.UseLoggerFactory(_sqlLoggerFactory);
            });
        }
    }
}