using DotNetCore.CAP;
using MediatR;

namespace FinancialChat.WebApi.Domain.Commands;

public class SearchStockInfo : IRequest
{
    public string StockCode { get; set; }
    public string Username { get; set; }

    public class SearchStockInfoCommandHandler : AsyncRequestHandler<SearchStockInfo>
    {
        private readonly ICapPublisher _bus;
        private readonly ILogger<SearchStockInfoCommandHandler> _logger;

        public SearchStockInfoCommandHandler(ICapPublisher bus, ILogger<SearchStockInfoCommandHandler> logger)
        {
            _bus    = bus;
            _logger = logger;
        }

        protected override Task Handle(SearchStockInfo command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending command to bot {@Command}", command);

            _bus.Publish("financialchat.bot", command);

            return Task.CompletedTask;
        }
    }
}