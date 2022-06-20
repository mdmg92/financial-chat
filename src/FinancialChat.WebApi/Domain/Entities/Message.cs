namespace FinancialChat.WebApi.Domain.Entities;

public class Message
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    public void SetTimestamp()
        => Timestamp = DateTime.Now;

    public bool IsBotCommand()
        => Text.StartsWith("/stock=");

    public string GetBotCommand()
        => IsBotCommand() ? Text.Replace("/stock=", string.Empty) : string.Empty;

    public bool IsNotice()
        => Text.StartsWith("[Notice]");
}
