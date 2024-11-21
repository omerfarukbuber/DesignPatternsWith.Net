using DesignPatterns.Base.Features;
using DesignPatterns.Base.Features.Products;
using Microsoft.EntityFrameworkCore;

namespace DesignPatterns.Base.Database;

public sealed class ApplicationDbContextSqlServer(DbContextOptions<ApplicationDbContextSqlServer> options)
    : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FeaturesAssembly).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}