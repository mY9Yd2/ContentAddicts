using ContentAddicts.Api.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ContentAddicts.UnitTests.Fixtures;

public class AppDbContextFixture
{
    private static readonly object Lock = new();
    private static bool s_databaseInitialized;
    private readonly string? _connectionString;

    public AppDbContextFixture()
    {
        lock (Lock)
        {
            _connectionString = GetConnectionString();

            if (!s_databaseInitialized)
            {
                InitializeDatabase();
                s_databaseInitialized = true;
            }
        }
    }

    public AppDbContext CreateContext()
    {
        return new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                    .UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString))
                    .Options);
    }

    private void InitializeDatabase()
    {
        using var context = CreateContext();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.SaveChanges();
    }

    private string GetConnectionString()
    {
        var builder = new ConfigurationBuilder()
                .AddUserSecrets<AppDbContextFixture>();

        var configuration = builder.Build();
        string? connectionString = configuration["UnitTestsConnectionString"];

        if (connectionString is null)
        {
            throw new ArgumentNullException(_connectionString, "UnitTestsConnectionString cannot be null!");
        }

        return connectionString;
    }
}
