using Backend.Domain.Chat;

namespace Backend.Domain.Tests.Chat;

public class ChatMessageTests
{
    [Fact]
    public void Create_ShouldCreateMessage_WhenInputIsValid()
    {
        var message = ChatMessage.Create(
            RoomId.Create("room-1"),
            AuthorName.Create("markus"),
            MessageText.Create("Hello world"));

        Assert.Equal(0, message.Id);
        Assert.Equal("room-1", message.RoomId.Value);
        Assert.Equal("markus", message.AuthorName.Value);
        Assert.Equal("Hello world", message.Text.Value);
        Assert.NotEqual(default, message.CreatedAtUtc);
        Assert.Equal(TimeSpan.Zero, message.CreatedAtUtc.Offset);
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
    public void Rehydrate_ShouldThrow_WhenTimestampIsMissing()
    {
        Assert.Throws<ArgumentException>(() =>
            ChatMessage.Rehydrate(
                1,
                RoomId.Create("room-1"),
                AuthorName.Create("markus"),
                MessageText.Create("Hello world"),
                default));
    }

    [Fact]
    public void Rehydrate_ShouldThrow_WhenTimestampOffsetIsNotUtc()
    {
        var createdAt = new DateTimeOffset(2026, 2, 27, 12, 0, 0, TimeSpan.FromHours(2));

        Assert.Throws<ArgumentException>(() =>
            ChatMessage.Rehydrate(
                1,
                RoomId.Create("room-1"),
                AuthorName.Create("markus"),
                MessageText.Create("Hello world"),
                createdAt));
    }
}
