using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _sqlLoggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFilter((category, level) =>
                            category == DbLoggerCategory.Database.Command.Name
                            && level == LogLevel.Information)
                        .AddConsole()   // when running from command prompt
                        .AddDebug();    // when debugging from VS
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Log.Logger);

            services.AddDbContext<RefactorThisDbContext>(options =>
            {
                options.UseSqlite("Data Source=../App_Data/products.db;");
                options.UseLoggerFactory(_sqlLoggerFactory);
            });
            services.AddControllers(options =>
            {
                options.Filters.Add<UnhandledExceptionFilter>();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RefactorThisAPI", Version = "v1" });
            });

            services.AddMediatR(typeof(GetProductRequest).Assembly);
            services.TryAddScoped<IProductRepository, ProductRepository>();
            services.TryAddScoped<IProductOptionRepository, ProductOptionRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<RequestLogger>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RefactorThisAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}