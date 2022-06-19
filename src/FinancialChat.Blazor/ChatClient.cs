using FinancialChat.Blazor.Models;

namespace FinancialChat.Blazor;

public class ChatClient
{
    private readonly HttpClient _client;

    public ChatClient(HttpClient client)
    {
        _client = client;
    }
    public async Task SendMesage(Message message)
        => await _client.PostAsJsonAsync("chat/messages", message);

    public async Task<IEnumerable<Message>> GetMessages()
        => (await _client.GetFromJsonAsync<IEnumerable<Message>>("chat/messages"))!;
}
