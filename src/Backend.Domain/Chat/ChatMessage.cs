namespace Backend.Domain.Chat;

public sealed record ChatMessage
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
        MessageText text)
    {
        return new ChatMessage(Guid.NewGuid(), roomId, authorName, text, DateTimeOffset.UtcNow);
    }

    public static ChatMessage Rehydrate(
        Guid id,
        RoomId roomId,
        AuthorName authorName,
        MessageText text,
        DateTimeOffset createdAtUtc)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id is required.", nameof(id));
        }

        ValidateTimestamp(createdAtUtc);
        return new ChatMessage(id, roomId, authorName, text, createdAtUtc);
    }

    private static void ValidateTimestamp(DateTimeOffset createdAtUtc)
    {
        if (createdAtUtc == default)
        {
            throw new ArgumentException("CreatedAtUtc is required.", nameof(createdAtUtc));
        }

        if (createdAtUtc.Offset != TimeSpan.Zero)
        {
            throw new ArgumentException("CreatedAtUtc must be UTC.", nameof(createdAtUtc));
        }

    }
}
