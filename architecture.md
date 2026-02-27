# Architecture (Stack Decisions)

## Frontend stack
- Chrome Extension
- React SPA
- Tailwind CSS
- Vite

## Backend stack
- .NET 8
- ASP.NET Core Minimal APIs
- SignalR (self-hosted first)
- Dapper (data access)
- Auth0-first external identity (provider-agnostic OIDC boundary)
- DbUp (database migrations)

## Data stack
- PostgreSQL
- Containerized PostgreSQL for local/dev/CI environments
- Azure Database for PostgreSQL Flexible Server (production)

## CI/CD stack
- GitHub Actions
- Container-based backend build artifacts
- Azure Container Registry
- Internal extension distribution (manual unpacked)

## Cloud target
- Azure Container Apps

## Platform decisions
- Environment variables for secrets/config (initially)
- No queue/event bus in v1
- No Redis in v1
- Minimal logging in v1
