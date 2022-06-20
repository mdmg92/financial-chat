using DotNetCore.CAP;
using FinancialChat.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.WebApi.Domain.Events;

public class StockInfoReceivedEvent
{
    public string Username { get; set; }
    public string Message { get; set; }
}

public class StockInfoReceivedEventHandler : ICapSubscribe
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<StockInfoReceivedEventHandler> _logger;

    public StockInfoReceivedEventHandler(IHubContext<ChatHub> hubContext, ILogger<StockInfoReceivedEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger     = logger;
    }

    [CapSubscribe("financialchat.api")]
    public async Task Handle(StockInfoReceivedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event received from bot {@Event}", @event);

        await _hubContext.Clients.All.SendAsync("Broadcast", @event.Username, @event.Message);
    }
}