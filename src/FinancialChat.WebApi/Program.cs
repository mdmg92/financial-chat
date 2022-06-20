using System.Reflection;
using FinancialChat.WebApi.Data;
using FinancialChat.WebApi.Domain.Events;
using FinancialChat.WebApi.Hubs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, lc)
    => lc.ReadFrom.Configuration(ctx.Configuration));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddCap(x =>
{
    x.UseInMemoryStorage();
    x.UseKafka(k =>
    {
        k.Servers = builder.Configuration.GetConnectionString("Kafka");
    });
});

builder.Services.AddTransient<StockInfoReceivedEventHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>(ChatHub.HubUrl);

app.Run();