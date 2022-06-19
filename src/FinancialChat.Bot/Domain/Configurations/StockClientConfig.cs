namespace FinancialChat.Bot.Domain.Services;

public record StockClientConfig
{
    public string QueryStringTemplate { get; set; }
    public string Endpoint { get; set; }
}