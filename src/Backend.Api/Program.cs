using Backend.Api;
using Backend.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBackendServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var migrationRunner = scope.ServiceProvider.GetRequiredService<DbUpMigrationRunner>();
    migrationRunner.RunMigrations();
}

app.MapBackendEndpoints();

app.Run();

public partial class Program;
