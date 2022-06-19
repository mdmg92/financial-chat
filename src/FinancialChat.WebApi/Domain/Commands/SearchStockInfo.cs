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

        public SearchStockInfoCommandHandler(ICapPublisher bus)
        {
            _bus = bus;
        }

        protected override Task Handle(SearchStockInfo command, CancellationToken cancellationToken)
        {
            _bus.Publish("financialchat.bot", command);

            return Task.CompletedTask;
        }
    }
}
