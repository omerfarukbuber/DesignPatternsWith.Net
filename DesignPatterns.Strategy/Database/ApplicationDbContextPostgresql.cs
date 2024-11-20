﻿using DesignPatterns.Strategy.Features;
using DesignPatterns.Strategy.Features.Products;
using Microsoft.EntityFrameworkCore;

namespace DesignPatterns.Strategy.Database;

public sealed class ApplicationDbContextPostgresql(DbContextOptions<ApplicationDbContextPostgresql> options)
    : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FeaturesAssembly).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}