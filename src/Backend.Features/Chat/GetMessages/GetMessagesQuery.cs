namespace Backend.Features.Chat.GetMessages;

public sealed record GetMessagesQuery(
    string? RoomId,
    int? PageSize,
    DateTimeOffset? BeforeUtc);

public sealed record GetMessagesItem(
    int Id,
    string RoomId,
    string AuthorName,
    string Text,
    DateTimeOffset CreatedAtUtc);

public sealed record GetMessagesResponse(
    IReadOnlyList<GetMessagesItem> Items,
    DateTimeOffset? NextBeforeUtc);
