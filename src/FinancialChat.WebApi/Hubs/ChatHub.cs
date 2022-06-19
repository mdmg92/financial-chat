using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.WebApi.Hubs;

public class ChatHub : Hub
{
    public const string HubUrl = "/chat";

    public async Task Broadcast(string username, string message)
    {
        await Clients.All.SendAsync("Broadcast", username, message);
    }
}
