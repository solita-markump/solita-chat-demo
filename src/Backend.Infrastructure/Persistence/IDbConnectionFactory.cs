using Npgsql;

namespace Backend.Infrastructure.Persistence;

public interface IDbConnectionFactory
{
    Task<NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken);
}
