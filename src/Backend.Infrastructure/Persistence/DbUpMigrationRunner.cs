using DbUp;

namespace Backend.Infrastructure.Persistence;

public sealed class DbUpMigrationRunner
{
    private readonly string _connectionString;

    public DbUpMigrationRunner(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Database connection string is required.", nameof(connectionString));
        }

        _connectionString = connectionString;
    }

    public void RunMigrations()
    {
        var upgrader =
            DeployChanges.To
                .PostgresqlDatabase(_connectionString)
                .WithScriptsEmbeddedInAssembly(
                    typeof(DbUpMigrationRunner).Assembly,
                    scriptName => scriptName.Contains(".Persistence.Migrations.Scripts.", StringComparison.Ordinal))
                .Build();

        var result = upgrader.PerformUpgrade();
        if (!result.Successful)
        {
            throw result.Error ?? new InvalidOperationException("Database migration failed.");
        }
    }
}
