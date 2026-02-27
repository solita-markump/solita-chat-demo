namespace Backend.Features.Chat.SendMessage;

public sealed record SendMessageRequest(
    string? RoomId,
    string? AuthorName,
    string? Text,
    DateTimeOffset? CreatedAtUtc);

public sealed record SendMessageResponse(
    Guid Id,
    string RoomId,
    string AuthorName,
    string Text,
    DateTimeOffset CreatedAtUtc);
