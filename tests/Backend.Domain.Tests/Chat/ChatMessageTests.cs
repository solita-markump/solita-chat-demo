using Backend.Domain.Chat;

namespace Backend.Domain.Tests.Chat;

public class ChatMessageTests
{
    [Fact]
    public void Create_ShouldCreateMessage_WhenInputIsValid()
    {
        var nowUtc = new DateTimeOffset(2026, 2, 27, 12, 0, 0, TimeSpan.Zero);
        var createdAtUtc = nowUtc.AddSeconds(-30);

        var message = ChatMessage.Create(
            RoomId.Create("room-1"),
            AuthorName.Create("markus"),
            MessageText.Create("Hello world"),
            createdAtUtc,
            nowUtc);

        Assert.NotEqual(Guid.Empty, message.Id);
        Assert.Equal("room-1", message.RoomId.Value);
        Assert.Equal("markus", message.AuthorName.Value);
        Assert.Equal("Hello world", message.Text.Value);
        Assert.Equal(createdAtUtc, message.CreatedAtUtc);
    }

    [Fact]
    public void Create_ShouldThrow_WhenRoomIdIsMissing()
    {
        Assert.Throws<ArgumentException>(() => RoomId.Create(" "));
    }

    [Fact]
    public void Create_ShouldThrow_WhenMessageTextIsTooLong()
    {
        var value = new string('x', MessageText.MaxLength + 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => MessageText.Create(value));
    }

    [Fact]
    public void Create_ShouldThrow_WhenTimestampIsTooFarInFuture()
    {
        var nowUtc = new DateTimeOffset(2026, 2, 27, 12, 0, 0, TimeSpan.Zero);
        var createdAtUtc = nowUtc.AddMinutes(2);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ChatMessage.Create(
                RoomId.Create("room-1"),
                AuthorName.Create("markus"),
                MessageText.Create("Hello world"),
                createdAtUtc,
                nowUtc));
    }

    [Fact]
    public void Create_ShouldThrow_WhenTimestampOffsetIsNotUtc()
    {
        var nowUtc = new DateTimeOffset(2026, 2, 27, 12, 0, 0, TimeSpan.Zero);
        var createdAt = new DateTimeOffset(2026, 2, 27, 12, 0, 0, TimeSpan.FromHours(2));

        Assert.Throws<ArgumentException>(() =>
            ChatMessage.Create(
                RoomId.Create("room-1"),
                AuthorName.Create("markus"),
                MessageText.Create("Hello world"),
                createdAt,
                nowUtc));
    }
}
