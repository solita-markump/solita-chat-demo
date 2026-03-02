using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Backend.Api.Tests.Infrastructure;

namespace Backend.Api.Tests.Chat;

public sealed class MessagesEndpointsTests : IClassFixture<PostgresApiFactory>
{
    private readonly PostgresApiFactory _factory;

    public MessagesEndpointsTests(PostgresApiFactory factory)
    {
        _factory = factory;
    }

    [DockerFact]
    public async Task PostMessages_ShouldReturnCreated_WhenPayloadIsValid()
    {
        using var client = await CreateClientAsync();

        var request = new SendMessageRequest(
            $"room-{Guid.NewGuid():N}",
            "markus",
            "hello world",
            DateTimeOffset.UtcNow);

        var response = await client.PostAsJsonAsync("/api/messages", request);
        var payload = await response.Content.ReadFromJsonAsync<SendMessageResponse>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(payload);
        Assert.NotEqual(Guid.Empty, payload.Id);
        Assert.Equal(request.RoomId, payload.RoomId);
    }

    [DockerFact]
    public async Task PostMessages_ShouldReturnValidationProblem_WhenRoomIdMissing()
    {
        using var client = await CreateClientAsync();

        var request = new SendMessageRequest(
            null,
            "markus",
            "hello world",
            DateTimeOffset.UtcNow);

        var response = await client.PostAsJsonAsync("/api/messages", request);
        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.True(payload.RootElement.GetProperty("errors").TryGetProperty("roomId", out _));
    }

    [DockerFact]
    public async Task GetMessages_ShouldReturnMessagesForRoom()
    {
        using var client = await CreateClientAsync();

        var roomId = $"room-{Guid.NewGuid():N}";

        await CreateMessageAsync(client, roomId, "first", DateTimeOffset.UtcNow.AddSeconds(-2));
        await CreateMessageAsync(client, roomId, "second", DateTimeOffset.UtcNow.AddSeconds(-1));
        await CreateMessageAsync(client, $"room-{Guid.NewGuid():N}", "other", DateTimeOffset.UtcNow);

        var response = await client.GetAsync($"/api/messages?roomId={Uri.EscapeDataString(roomId)}&pageSize=10");
        var payload = await response.Content.ReadFromJsonAsync<GetMessagesResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(payload);
        Assert.True(payload.Items.Count >= 2);
        Assert.All(payload.Items, item => Assert.Equal(roomId, item.RoomId));
    }

    [DockerFact]
    public async Task GetMessages_ShouldReturnValidationProblem_WhenRoomIdMissing()
    {
        using var client = await CreateClientAsync();

        var response = await client.GetAsync("/api/messages?pageSize=10");
        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.True(payload.RootElement.GetProperty("errors").TryGetProperty("roomId", out _));
    }

    private async Task<HttpClient> CreateClientAsync()
    {
        await _factory.EnsureStartedAsync();
        return _factory.CreateClient();
    }

    private static async Task CreateMessageAsync(HttpClient client, string roomId, string text, DateTimeOffset createdAtUtc)
    {
        var response = await client.PostAsJsonAsync(
            "/api/messages",
            new SendMessageRequest(roomId, "markus", text, createdAtUtc));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    private sealed record SendMessageRequest(
        string? RoomId,
        string? AuthorName,
        string? Text,
        DateTimeOffset? CreatedAtUtc);

    private sealed record SendMessageResponse(
        Guid Id,
        string RoomId,
        string AuthorName,
        string Text,
        DateTimeOffset CreatedAtUtc);

    private sealed record GetMessagesResponse(
        IReadOnlyList<GetMessagesItem> Items,
        DateTimeOffset? NextBeforeUtc);

    private sealed record GetMessagesItem(
        Guid Id,
        string RoomId,
        string AuthorName,
        string Text,
        DateTimeOffset CreatedAtUtc);
}
