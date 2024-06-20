using ContentAddicts.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace ContentAddicts.Api.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Creator> Creators { get; set; }
}
