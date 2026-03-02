using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Backend.Api.Tests.Infrastructure;

public sealed class PostgresApiFactory : WebApplicationFactory<Program>, IAsyncDisposable
{
    private PostgreSqlContainer? _container;
    private string _connectionString = "Host=localhost;Port=5432;Database=chatdb;Username=postgres;Password=postgres";

    public async Task EnsureStartedAsync()
    {
        if (_container is not null)
        {
            return;
        }

        _container = new PostgreSqlBuilder("postgres:16-alpine")
            .WithDatabase("chatdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        await _container.StartAsync();
        _connectionString = _container.GetConnectionString();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DATABASE_CONNECTION_STRING"] = _connectionString
            });
        });
    }

    public new async ValueTask DisposeAsync()
    {
        if (_container is not null)
        {
            await _container.DisposeAsync();
        }

        await base.DisposeAsync();
    }
}
