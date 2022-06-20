using FinancialChat.WebApi.Data;
using FinancialChat.WebApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.WebApi.Domain.Queries;

public class GetMessages : IRequest<IEnumerable<Message>>
{
    public class GetMessagesQueryHandler : IRequestHandler<GetMessages, IEnumerable<Message>>
    {
        private readonly ChatDbContext _db;

        public GetMessagesQueryHandler(ChatDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Message>> Handle(GetMessages request, CancellationToken cancellationToken) =>
            await _db.Messages
                .OrderByDescending(t => t.Timestamp)
                .Take(50)
                .ToListAsync(cancellationToken: cancellationToken);
    }
}
