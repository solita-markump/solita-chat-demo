using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Backend.Features.Chat.GetMessages;

public static class GetMessagesEndpoint
{
    public static IEndpointRouteBuilder MapGetMessagesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/messages", HandleAsync)
            .WithTags("Chat")
            .WithSummary("Get messages")
            .WithDescription("Returns paged chat messages for a room.")
            .Produces<GetMessagesResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        string? roomId,
        int? pageSize,
        DateTimeOffset? beforeUtc,
        GetMessagesHandler handler,
        CancellationToken cancellationToken)
    {
        var errors = Validate(roomId, pageSize);
        if (errors.Count > 0)
        {
            return Results.ValidationProblem(errors);
        }

        try
        {
            var response = await handler.HandleAsync(
                new GetMessagesQuery(roomId, pageSize, beforeUtc),
                cancellationToken);

            return Results.Ok(response);
        }
        catch (ArgumentException ex)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["message"] = new[] { ex.Message }
            });
        }
    }

    private static Dictionary<string, string[]> Validate(string? roomId, int? pageSize)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(roomId))
        {
            errors["roomId"] = new[] { "roomId is required." };
        }

        if (pageSize is <= 0 or > 200)
        {
            errors["pageSize"] = new[] { "pageSize must be between 1 and 200 when provided." };
        }

        return errors;
    }
}
