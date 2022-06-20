using FinancialChat.WebApi.Data;
using FinancialChat.WebApi.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.WebApi.Domain.Commands;

public class NewMessage : IRequest
{
    public string Username { get; set; }
    public string Text { get; set; }

    public class MessageReceivedEventHandler : AsyncRequestHandler<NewMessage>
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ChatDbContext _db;
        private readonly IMediator _mediator;
        private readonly ILogger<MessageReceivedEventHandler> _logger;

        public MessageReceivedEventHandler(IHubContext<ChatHub> hubContext, ChatDbContext db, IMediator mediator,
            ILogger<MessageReceivedEventHandler> logger)
        {
            _hubContext = hubContext;
            _db         = db;
            _mediator   = mediator;
            _logger     = logger;
        }

        protected override async Task Handle(NewMessage @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("New message received {@Message}", @event);
            var chatMessage = new Message
            {
                Username = @event.Username,
                Text     = @event.Text
            };

            if (chatMessage.IsBotCommand())
            {
                _logger.LogInformation("--- Sending command to bot");
                await _mediator.Send(new SearchStockInfo
                {
                    Username  = @event.Username,
                    StockCode = chatMessage.GetBotCommand()
                }, cancellationToken);
            }

            if (!chatMessage.IsBotCommand() && !chatMessage.IsNotice())
            {
                _logger.LogInformation("--- Saving message to database");
                chatMessage.SetTimestamp();
                _db.Messages.Add(chatMessage);
                await _db.SaveChangesAsync();
            }

            _logger.LogInformation("Broadcasting new message");
            await _hubContext.Clients.All.SendAsync("Broadcast", @event.Username, @event.Text);
        }
    }
}