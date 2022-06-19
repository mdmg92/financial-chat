using Microsoft.EntityFrameworkCore;

namespace FinancialChat.WebApi.Data;

public class ChatDbContext : DbContext
{
    public DbSet<Message> Messages { get; set; }

    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options)
    {
    }
}
