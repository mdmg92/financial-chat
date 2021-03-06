using FinancialChat.Bot.Domain.Commands;
using FinancialChat.Bot.Domain.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, lc)
    => lc.ReadFrom.Configuration(ctx.Configuration));

// Add services to the container.
builder.Services.AddTransient<ISearchStockInfoCommandHandler, SearchStockInfoCommandHandler>();

builder.Services.Configure<StockClientConfig>(builder.Configuration.GetSection(nameof(StockClientConfig)));

builder.Services.AddHttpClient<StockClient>();

builder.Services.AddCap(x =>
{
    x.UseInMemoryStorage();
    x.UseKafka(k =>
    {
        k.Servers = builder.Configuration.GetConnectionString("Kafka");
    });
});

var app = builder.Build();

app.Run();