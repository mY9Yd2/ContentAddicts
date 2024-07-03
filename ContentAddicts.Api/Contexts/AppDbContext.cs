using ContentAddicts.Api.Converters;
using ContentAddicts.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace ContentAddicts.Api.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<Creator> Creators { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
                .Properties<Sex>()
                .HaveConversion<SexConverter>();
    }
}
