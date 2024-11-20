using Carter;
using DesignPatterns.Strategy.Database;
using DesignPatterns.Strategy.Database;
using DesignPatterns.Strategy.Features.Products;
using DesignPatterns.Strategy.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IProductRepository>(serviceProvider =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var strategyType = httpContextAccessor.HttpContext?.Request.Cookies["StrategyDesignPatternType"];
    strategyType = string.IsNullOrWhiteSpace(strategyType) ? ((StrategyDesignPatternType)0).ToString() : strategyType;

    return strategyType switch
    {
        nameof(StrategyDesignPatternType.SqlServer) => new ProductRepositoryWithSqlServer(
            serviceProvider.GetRequiredService<ApplicationDbContextSqlServer>()),
        nameof(StrategyDesignPatternType.Postgres) => new ProductRepositoryWithPostgresql(
            serviceProvider.GetRequiredService<ApplicationDbContextPostgresql>()),
        nameof(StrategyDesignPatternType.MongoDb) => new ProductRepositoryWthMongoDb(builder.Configuration),
        _ => throw new ArgumentOutOfRangeException()
    };
});

builder.Services.AddDbContext<ApplicationDbContextSqlServer>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddDbContext<ApplicationDbContextPostgresql>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapCarter();

app.Run();
