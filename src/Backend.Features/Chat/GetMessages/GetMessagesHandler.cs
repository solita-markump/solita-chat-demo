using Backend.Domain.Chat;

namespace Backend.Features.Chat.GetMessages;

public sealed class GetMessagesHandler
{
    private readonly IChatMessageRepository _repository;

    public GetMessagesHandler(IChatMessageRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetMessagesResponse> HandleAsync(GetMessagesQuery query, CancellationToken cancellationToken)
    {
        var pageSize = query.PageSize ?? 50;

        var messages = await _repository.GetByRoomAsync(
            RoomId.Create(query.RoomId!),
            pageSize,
            query.BeforeUtc,
            cancellationToken);

        var items = messages
            .Select(message => new GetMessagesItem(
                message.Id,
                message.RoomId.Value,
                message.AuthorName.Value,
                message.Text.Value,
                message.CreatedAtUtc))
            .ToList();

        DateTimeOffset? nextBeforeUtc = items.Count > 0 ? items[^1].CreatedAtUtc : null;
        return new GetMessagesResponse(items, nextBeforeUtc);
    }
}
