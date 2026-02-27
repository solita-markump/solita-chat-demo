using Backend.Domain.Chat;
using Backend.Features.Chat.GetMessages;
using Backend.Features.Chat.SendMessage;
using Backend.Infrastructure.Persistence;

namespace Backend.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddBackendServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            configuration["DATABASE_CONNECTION_STRING"] ??
            configuration.GetConnectionString("Chat");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string is missing. Set DATABASE_CONNECTION_STRING or ConnectionStrings:Chat.");
        }

        services.AddSingleton(new DatabaseOptions
        {
            ConnectionString = connectionString
        });

        services.AddSingleton<IDbConnectionFactory>(provider =>
            new NpgsqlConnectionFactory(provider.GetRequiredService<DatabaseOptions>().ConnectionString));
        services.AddScoped<IChatMessageRepository, DapperChatMessageRepository>();

        services.AddScoped<SendMessageHandler>();
        services.AddScoped<GetMessagesHandler>();

        services.AddSingleton(provider =>
            new DbUpMigrationRunner(provider.GetRequiredService<DatabaseOptions>().ConnectionString));

        return services;
    }
}
