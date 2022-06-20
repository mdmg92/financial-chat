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
    private static string ErrorTemplate = "An error ocurred while requesting stock info for {0}: {1}";

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
        _logger.LogInformation("Request to search stock info received {@Command}", command);

        var message = string.Empty;

        try
        {
            var info = await _client.DownloadStockInfo(command.StockCode);

            if (info is null)
            {
                _logger.LogInformation("Stock information not found");
                return;
            }

            message = GetFormattedMessage(info);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred");
            message = GetErrorMessage(command.StockCode, ex.Message);
        }

        _logger.LogInformation("Publishing message to topic {Message}", message);
        _bus.Publish("financialchat.api", new StockInfoReceivedEvent
        {
            Username = command.Username,
            Message = message
        });
    }

    private static string GetFormattedMessage(StockInfo info)
        => string.Format(MessageTemplate, info.Symbol.ToUpper(), info.Close);

    private static string GetErrorMessage(string code, string message)
        => string.Format(MessageTemplate, code, message);
}
