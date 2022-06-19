namespace FinancialChat.Blazor.Models;

public class Message
{
    public Message(string username, string text, bool mine)
    {
        Username = username;
        Text     = text;
        Mine     = mine;
    }

    public bool Mine { get; set; }

    public string Text { get; set; }

    public string Username { get; set; }

    public bool IsNotice => Text.StartsWith("[Notice]");
    public string CSS => Mine ? "sent" : "received";
}