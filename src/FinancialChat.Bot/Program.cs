var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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