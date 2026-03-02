using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Backend.Features.Chat.SendMessage;

public static class SendMessageEndpoint
{
    public static IEndpointRouteBuilder MapSendMessageEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/messages", HandleAsync);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        SendMessageRequest request,
        SendMessageHandler handler,
        CancellationToken cancellationToken)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            return Results.ValidationProblem(errors);
        }

        try
        {
            var message = await handler.HandleAsync(request, cancellationToken);
            return Results.Created(
                $"/api/messages/{message.Id}",
                new SendMessageResponse(
                    message.Id,
                    message.RoomId.Value,
                    message.AuthorName.Value,
                    message.Text.Value,
                    message.CreatedAtUtc));
        }
        catch (ArgumentException ex)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["message"] = new[] { ex.Message }
            });
        }
    }

    private static Dictionary<string, string[]> Validate(SendMessageRequest request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(request.RoomId))
        {
            errors["roomId"] = new[] { "roomId is required." };
        }

        if (string.IsNullOrWhiteSpace(request.AuthorName))
        {
            errors["authorName"] = new[] { "authorName is required." };
        }

        if (string.IsNullOrWhiteSpace(request.Text))
        {
            errors["text"] = new[] { "text is required." };
        }

        return errors;
    }
}
