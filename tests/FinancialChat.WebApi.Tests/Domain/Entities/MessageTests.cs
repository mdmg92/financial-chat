using FinancialChat.WebApi.Data;
using FluentAssertions;

namespace FinancialChat.WebApi.Tests.Domain.Entities;

public class MessageTests
{
    [Test]
    public void IsBotCommand_WhenMessageStartWithStock_ReturnsTrue()
    {
        // Arrange
        var message = new Message
        {
            Text = "/stock="
        };

        // Act
        var result = message.IsBotCommand();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsBotCommand_WhenMessageDoesNotStartWithStock_ReturnsFalse()
    {
        // Arrange
        var message = new Message
        {
            Text = ""
        };

        // Act
        var result = message.IsBotCommand();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void IsNotice_WhenMessageStartWithNotice_ReturnsTrue()
    {
        // Arrange
        var message = new Message
        {
            Text = "[Notice]"
        };

        // Act
        var result = message.IsNotice();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsNotice_WhenMessageDoesNotStartWithNotice_ReturnsFalse()
    {
        // Arrange
        var message = new Message
        {
            Text = ""
        };

        // Act
        var result = message.IsNotice();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void SetTimestamp_WhenCalled_SetTimestampToCurrentDate()
    {
        var message = new Message();

        var oldValue = message.Timestamp;

        message.SetTimestamp();

        message.Timestamp.Should().NotBeBefore(oldValue);
        message.Timestamp.Should().BeAfter(oldValue);
    }

    [Test]
    public void GetBotCommand_WhenIsBotCommandIsFalse_ReturnsEmptyString()
    {
        var message = new Message();

        var result = message.GetBotCommand();

        result.Should().BeEmpty();
    }

    [Test]
    public void GetBotCommand_WhenIsBotCommandIsTrue_ReturnsPosibleStockCode()
    {
        var stockCode = "CODE";
        var message = new Message
        {
            Text = $"/stock={stockCode}"
        };

        var result = message.GetBotCommand();

        result.Should().NotBeEmpty();
        result.Should().Be(stockCode);
    }
}
