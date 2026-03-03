using Backend.Domain.Chat;
using Dapper;

namespace Backend.Infrastructure.Persistence;

public sealed class DapperChatMessageRepository : IChatMessageRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DapperChatMessageRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<ChatMessage> SaveAsync(ChatMessage message, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO messages (id, room_id, author_name, message_text)
            VALUES (@Id, @RoomId, @AuthorName, @MessageText)
            RETURNING created_at_utc;
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var command = new CommandDefinition(
            sql,
            new
            {
                message.Id,
                RoomId = message.RoomId.Value,
                AuthorName = message.AuthorName.Value,
                MessageText = message.Text.Value
            },
            cancellationToken: cancellationToken);

        var createdAtUtc = await connection.QuerySingleAsync<DateTime>(command);
        return ChatMessage.Rehydrate(
            message.Id,
            message.RoomId,
            message.AuthorName,
            message.Text,
            ToUtcDateTimeOffset(createdAtUtc));
    }

    public async Task<IReadOnlyList<ChatMessage>> GetByRoomAsync(
        RoomId roomId,
        int pageSize,
        DateTimeOffset? beforeUtc,
        CancellationToken cancellationToken)
    {
        if (pageSize <= 0 || pageSize > 200)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 200.");
        }

        if (beforeUtc.HasValue && beforeUtc.Value.Offset != TimeSpan.Zero)
        {
            throw new ArgumentException("beforeUtc must use UTC offset.", nameof(beforeUtc));
        }

        const string sql = """
            SELECT
                id AS Id,
                room_id AS RoomId,
                author_name AS AuthorName,
                message_text AS MessageText,
                created_at_utc AS CreatedAtUtc
            FROM messages
            WHERE room_id = @RoomId
              AND (@BeforeUtc IS NULL OR created_at_utc < @BeforeUtc)
            ORDER BY created_at_utc DESC, id DESC
            LIMIT @PageSize;
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var command = new CommandDefinition(
            sql,
            new
            {
                RoomId = roomId.Value,
                BeforeUtc = beforeUtc,
                PageSize = pageSize
            },
            cancellationToken: cancellationToken);

        var rows = await connection.QueryAsync<MessageRow>(command);
        return rows
            .Select(row => ChatMessage.Rehydrate(
                row.Id,
                RoomId.Create(row.RoomId),
                AuthorName.Create(row.AuthorName),
                MessageText.Create(row.MessageText),
                ToUtcDateTimeOffset(row.CreatedAtUtc)))
            .ToList();
    }

    private static DateTimeOffset ToUtcDateTimeOffset(DateTime value)
    {
        var utcDateTime = value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };

        return new DateTimeOffset(utcDateTime);
    }

    private sealed record MessageRow(
        Guid Id,
        string RoomId,
        string AuthorName,
        string MessageText,
        DateTime CreatedAtUtc);
}
