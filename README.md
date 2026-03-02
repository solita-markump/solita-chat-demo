# solita-chat-demo
Between projects work: A chrome extension where every page you are on is a chat room

## Backend minimal v1

### Prerequisites
- .NET 8 SDK
- PostgreSQL (Docker recommended)

### Start PostgreSQL locally (Docker example)
```powershell
docker run --name solita-chat-postgres `
  -e POSTGRES_USER=postgres `
  -e POSTGRES_PASSWORD=postgres `
  -e POSTGRES_DB=chatdb `
  -p 5432:5432 `
  -d postgres:16-alpine
```

### Configure and run backend
```powershell
$env:DATABASE_CONNECTION_STRING="Host=localhost;Port=5432;Database=chatdb;Username=postgres;Password=postgres"
dotnet run --project src\Backend.Api\Backend.Api.csproj
```

Backend API endpoints:
- `POST /api/messages`
- `GET /api/messages?roomId=<roomId>&pageSize=50&beforeUtc=<ISO-8601>`
- `GET /health`

DbUp migrations are applied automatically on API startup.

### Run tests
```powershell
dotnet test tests\Backend.Domain.Tests\Backend.Domain.Tests.csproj
dotnet test tests\Backend.Api.Tests\Backend.Api.Tests.csproj
```

`Backend.Api.Tests` uses PostgreSQL testcontainers for integration coverage and is skipped when Docker is unavailable.
