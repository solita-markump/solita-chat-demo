using Backend.Domain.Chat;

namespace Backend.Features.Chat.SendMessage;

public sealed class SendMessageHandler
{
    private readonly IChatMessageRepository _repository;

    public SendMessageHandler(IChatMessageRepository repository)
    {
        _repository = repository;
    }

    public async Task<ChatMessage> HandleAsync(SendMessageRequest request, CancellationToken cancellationToken)
    {
        var message = ChatMessage.Create(
            RoomId.Create(request.RoomId!),
            AuthorName.Create(request.AuthorName!),
            MessageText.Create(request.Text!),
            request.CreatedAtUtc!.Value);

        await _repository.SaveAsync(message, cancellationToken);
        return message;
    }
}
