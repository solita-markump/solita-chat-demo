using Npgsql;

namespace Backend.Infrastructure.Persistence;

public sealed class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Database connection string is required.", nameof(connectionString));
        }

        _connectionString = connectionString;
    }

    public async Task<NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
