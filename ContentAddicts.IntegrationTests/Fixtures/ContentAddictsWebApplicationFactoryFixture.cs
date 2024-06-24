using ContentAddicts.Api.Contexts;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Respawn;

namespace ContentAddicts.IntegrationTests.Fixtures;

public class ContentAddictsWebApplicationFactoryFixture<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly string _connectionString;

    public ContentAddictsWebApplicationFactoryFixture()
    {
        _connectionString = GetConnectionString();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>));


            if (dbContextDescriptor is not null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
            });

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<AppDbContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.SaveChanges();
        });

        builder.UseEnvironment("Development");
    }

    private string GetConnectionString()
    {
        var builder = new ConfigurationBuilder()
                .AddUserSecrets<ContentAddictsWebApplicationFactoryFixture<TProgram>>();

        var configuration = builder.Build();
        string? connectionString = configuration["TestIntegrationDefaultConnectionString"];

        if (connectionString is null)
        {
            throw new ArgumentNullException(_connectionString, "TestIntegrationDefaultConnectionString cannot be null!");
        }

        return connectionString;
    }

    public async Task ResetDatabaseAsync(AppDbContext context)
    {
        var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();

        var respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.MySql
        });

        await respawner.ResetAsync(connection);
    }
}
