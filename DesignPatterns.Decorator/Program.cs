using Carter;
using DesignPatterns.Decorator.Database;
using DesignPatterns.Decorator.Features.Products;
using DesignPatterns.Decorator.Features.Products.Decorators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();

builder.Services.AddStackExchangeRedisCache(setup =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    setup.Configuration = connection;
});

builder.Services.AddDbContext<ApplicationDbContextSqlServer>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddDbContext<ApplicationDbContextPostgresql>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

//Add Scrutor library --> 
builder.Services.AddScoped<IProductRepository, ProductRepository>()
    .Decorate<IProductRepository, ProductRepositoryCacheDecorator>();

//builder.Services.AddScoped<IProductRepository>(provider =>
//{
//    var productRepository = new ProductRepository(provider.GetRequiredService<ApplicationDbContextSqlServer>());
//    var productRepositoryCacheDecorator = new ProductRepositoryCacheDecorator(productRepository,
//        provider.GetRequiredService<IDistributedCache>());
//    return productRepositoryCacheDecorator;
//});

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
