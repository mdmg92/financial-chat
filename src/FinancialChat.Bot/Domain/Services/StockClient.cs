using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Options;

namespace FinancialChat.Bot.Domain.Services;

public class StockClient
{
    private readonly HttpClient _client;
    private readonly StockClientConfig _config;

    public StockClient(HttpClient client, IOptions<StockClientConfig> config)
    {
        _client = client;
        _config = config.Value;
    }

    public async Task<StockInfo> DownloadStockInfo(string stockCode)
    {
        var queryString = string.Format(_config.QueryStringTemplate, stockCode);
        var uri = new Uri($"{_config.Endpoint}?{queryString}");

        var result = await _client.GetAsync(uri);
        result.EnsureSuccessStatusCode();

        var stream = await result.Content.ReadAsStreamAsync();
        var info = StockInfoMap.Map(stream);

        return info;
    }
}

public record StockInfo
{
    public string Symbol { get; set; }
    public string Close { get; set; }
}

public static class StockInfoMap
{
    public static StockInfo Map(Stream csvStream)
    {
        using (var reader = new StreamReader(csvStream))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Read();
            var info = csv.GetRecord<StockInfo>();

            return info;
        }
    }
}
