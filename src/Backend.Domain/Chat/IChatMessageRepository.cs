namespace Backend.Domain.Chat;

public interface IChatMessageRepository
{
    Task<ChatMessage> SaveAsync(ChatMessage message, CancellationToken cancellationToken);

    Task<IReadOnlyList<ChatMessage>> GetByRoomAsync(
        RoomId roomId,
        int pageSize,
        DateTimeOffset? beforeUtc,
        CancellationToken cancellationToken);
}
