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
            "hello world");

        var response = await client.PostAsJsonAsync("/api/messages", request);
        var payload = await response.Content.ReadFromJsonAsync<SendMessageResponse>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(payload);
        Assert.True(payload.Id > 0);
        Assert.Equal(request.RoomId, payload.RoomId);
        Assert.NotEqual(default, payload.CreatedAtUtc);
        Assert.Equal(TimeSpan.Zero, payload.CreatedAtUtc.Offset);
    }

    [DockerFact]
    public async Task PostMessages_ShouldReturnValidationProblem_WhenRoomIdMissing()
    {
        using var client = await CreateClientAsync();

        var request = new SendMessageRequest(
            null,
            "markus",
            "hello world");

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

        await CreateMessageAsync(client, roomId, "first");
        await CreateMessageAsync(client, roomId, "second");
        await CreateMessageAsync(client, $"room-{Guid.NewGuid():N}", "other");

        var response = await client.GetAsync($"/api/messages?roomId={Uri.EscapeDataString(roomId)}&pageSize=10");
        var payload = await response.Content.ReadFromJsonAsync<GetMessagesResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(payload);
        Assert.True(payload.Items.Count >= 2);
        Assert.All(payload.Items, item => Assert.Equal(roomId, item.RoomId));
        Assert.Equal(payload.Items[^1].CreatedAtUtc, payload.NextBeforeUtc);
        Assert.True(payload.Items.Zip(payload.Items.Skip(1), (previous, current) => previous.CreatedAtUtc >= current.CreatedAtUtc).All(isOrdered => isOrdered));
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

    private static async Task CreateMessageAsync(HttpClient client, string roomId, string text)
    {
        var response = await client.PostAsJsonAsync(
            "/api/messages",
            new SendMessageRequest(roomId, "markus", text));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    private sealed record SendMessageRequest(
        string? RoomId,
        string? AuthorName,
        string? Text);

    private sealed record SendMessageResponse(
        int Id,
        string RoomId,
        string AuthorName,
        string Text,
        DateTimeOffset CreatedAtUtc);

    private sealed record GetMessagesResponse(
        IReadOnlyList<GetMessagesItem> Items,
        DateTimeOffset? NextBeforeUtc);

    private sealed record GetMessagesItem(
        int Id,
        string RoomId,
        string AuthorName,
        string Text,
        DateTimeOffset CreatedAtUtc);
}
