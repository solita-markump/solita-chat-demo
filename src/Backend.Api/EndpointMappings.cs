using Backend.Features.Chat.GetMessages;
using Backend.Features.Chat.SendMessage;

namespace Backend.Api;

public static class EndpointMappings
{
    public static IEndpointRouteBuilder MapBackendEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapSendMessageEndpoint();
        endpoints.MapGetMessagesEndpoint();
        endpoints.MapGet("/health", () => Results.Ok(new { status = "ok" }));

        return endpoints;
    }
}
