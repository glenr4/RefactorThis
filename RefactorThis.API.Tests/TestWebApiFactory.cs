using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefactorThis.Persistence.Sqlite;
using System;
using System.Linq;

namespace RefactorThis.API.Tests
{
    public class TestWebApiFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<RefactorThisDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<RefactorThisDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<RefactorThisDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<TestWebApiFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        TestDbSeeding.Init(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}