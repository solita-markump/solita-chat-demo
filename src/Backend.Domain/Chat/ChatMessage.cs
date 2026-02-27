namespace Backend.Domain.Chat;

public sealed class ChatMessage
{
    public Guid Id { get; }

    public RoomId RoomId { get; }

    public AuthorName AuthorName { get; }

    public MessageText Text { get; }

    public DateTimeOffset CreatedAtUtc { get; }

    private ChatMessage(Guid id, RoomId roomId, AuthorName authorName, MessageText text, DateTimeOffset createdAtUtc)
    {
        Id = id;
        RoomId = roomId;
        AuthorName = authorName;
        Text = text;
        CreatedAtUtc = createdAtUtc;
    }

    public static ChatMessage Create(
        RoomId roomId,
        AuthorName authorName,
        MessageText text,
        DateTimeOffset createdAtUtc,
        DateTimeOffset? nowUtc = null)
    {
        if (createdAtUtc == default)
        {
            throw new ArgumentException("CreatedAtUtc is required.", nameof(createdAtUtc));
        }

        if (createdAtUtc.Offset != TimeSpan.Zero)
        {
            throw new ArgumentException("CreatedAtUtc must be UTC.", nameof(createdAtUtc));
        }

        var now = nowUtc ?? DateTimeOffset.UtcNow;
        if (createdAtUtc > now.AddMinutes(1))
        {
            throw new ArgumentOutOfRangeException(nameof(createdAtUtc), "CreatedAtUtc cannot be more than one minute in the future.");
        }

        return new ChatMessage(Guid.NewGuid(), roomId, authorName, text, createdAtUtc);
    }
}
