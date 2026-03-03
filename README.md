# solita-chat-demo
Between projects work: A chrome extension where every page you are on is a chat room

## Backend minimal v1

### Prerequisites
- .NET 8 SDK
- PostgreSQL (Docker recommended)

### Start PostgreSQL locally (Docker Compose)
```powershell
docker compose up -d
```

Database data is persisted in the `solita-chat-postgres-data` named volume from `docker-compose.yml`.

### Run backend
```powershell
dotnet run --project src\Backend.Api\Backend.Api.csproj
```

Local development uses `ConnectionStrings:Chat` in `src\Backend.Api\appsettings.Development.json`.
To override it for the current shell, set `DATABASE_CONNECTION_STRING` before running:
```powershell
$env:DATABASE_CONNECTION_STRING="Host=localhost;Port=55432;Database=chatdb;Username=postgres;Password=postgres"
dotnet run --project src\Backend.Api\Backend.Api.csproj
```

Backend API endpoints:
- `POST /api/messages`
- `GET /api/messages?roomId=<roomId>&pageSize=50&beforeUtc=<ISO-8601>`
- `GET /health`

Development-only API documentation:
- OpenAPI JSON: `GET /openapi/v1.json`
- Scalar UI: `/scalar/v1`

The OpenAPI and Scalar endpoints are only mapped when running in `Development`.

DbUp migrations are applied automatically on API startup.

### Run tests
```powershell
dotnet test tests\Backend.Domain.Tests\Backend.Domain.Tests.csproj
dotnet test tests\Backend.Api.Tests\Backend.Api.Tests.csproj
```

`Backend.Api.Tests` uses PostgreSQL testcontainers for integration coverage and is skipped when Docker is unavailable.
