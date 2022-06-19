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

        public MessageReceivedEventHandler(IHubContext<ChatHub> hubContext, ChatDbContext db, IMediator mediator)
        {
            _hubContext = hubContext;
            _db         = db;
            _mediator   = mediator;
        }

        protected override async Task Handle(NewMessage @event, CancellationToken cancellationToken)
        {
            var chatMessage = new Message
            {
                Username = @event.Username,
                Text     = @event.Text
            };

            if (!chatMessage.IsBotCommand() && !chatMessage.IsNotice())
            {
                chatMessage.SetTimestamp();
                _db.Messages.Add(chatMessage);
                await _db.SaveChangesAsync();
            }

            await _hubContext.Clients.All.SendAsync("Broadcast", @event.Username, @event.Text);
        }
    }
}
