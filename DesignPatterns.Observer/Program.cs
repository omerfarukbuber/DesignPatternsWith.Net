using Carter;
using DesignPatterns.Observer.Database;
using DesignPatterns.Observer.Features;
using DesignPatterns.Observer.Features.Products.Observers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();

builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssembly(typeof(FeaturesAssembly).Assembly));

builder.Services.AddSingleton<IProductPriceChangeSubject>(sp =>
{
    var productPriceChangeSubject = new ProductPriceChangeSubject();

    productPriceChangeSubject.RegisterObserver(
        new ProductPriceChangeObserverUpdateBasket(sp
            .GetRequiredService<ILogger<ProductPriceChangeObserverUpdateBasket>>()));
    productPriceChangeSubject.RegisterObserver(
        new ProductPriceChangeObserverSendNotificationToUser(sp
            .GetRequiredService<ILogger<ProductPriceChangeObserverSendNotificationToUser>>()));

    return productPriceChangeSubject;
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
