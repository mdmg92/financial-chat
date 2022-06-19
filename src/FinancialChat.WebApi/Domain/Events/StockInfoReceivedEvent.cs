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
    private readonly ChatHub _chatHub;

    public StockInfoReceivedEventHandler(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [CapSubscribe("financialchat.api")]
    public async Task Handle(StockInfoReceivedEvent @event, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.All.SendAsync("Broadcast", @event.Username, @event.Message);
    }
}