using DotNetCore.CAP;
using FinancialChat.Bot.Domain.Services;

namespace FinancialChat.Bot.Domain.Commands;

public class SearchStockInfoCommand
{
    public string StockCode { get; set; }
    public string Username { get; set; }
}

public class StockInfoReceivedEvent
{
    public string Username { get; set; }
    public string Message { get; set; }
}

public interface ISearchStockInfoCommandHandler
{
    Task Handle(SearchStockInfoCommand command);
}

public class SearchStockInfoCommandHandler : ICapSubscribe, ISearchStockInfoCommandHandler
{
    private readonly StockClient _client;
    private readonly ICapPublisher _bus;
    private readonly ILogger<SearchStockInfoCommandHandler> _logger;
    private static string MessageTemplate = "{0} quote is ${1} per share";

    public SearchStockInfoCommandHandler(StockClient client, ICapPublisher bus,
        ILogger<SearchStockInfoCommandHandler> logger)
    {
        _client = client;
        _bus    = bus;
        _logger = logger;
    }

    [CapSubscribe("financialchat.bot")]
    public async Task Handle(SearchStockInfoCommand command)
    {
        var info = await _client.DownloadStockInfo(command.StockCode);

        if (info is null)
        {
            return;
        }

        var message = GetFormattedMessage(info);

        _bus.Publish("financialchat.api", new StockInfoReceivedEvent
        {
            Username = command.Username,
            Message = message
        });
    }

    private static string GetFormattedMessage(StockInfo info)
        => string.Format(MessageTemplate, info.Symbol.ToUpper(), info.Close);
}
