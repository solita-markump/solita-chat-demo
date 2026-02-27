# Backend Implementation Plan (Minimal v1)

## Current state
- Architecture decisions are defined in `architecture.md`.
- Backend stack target: .NET 8, ASP.NET Core Minimal APIs, SignalR, Dapper, DbUp.
- Data target: PostgreSQL (containerized for local/dev/CI).
- Platform constraints for v1: env-var config, no queue/event bus, no Redis, minimal logging.
- Repository currently contains docs only; no backend source scaffolding is present yet.

## Problem and approach
Build the smallest backend that can **send and receive chat data** while establishing a maintainable foundation:
- Domain-driven design for core rules.
- Slice (vertical feature) architecture for use-case isolation.
- Tests created from the beginning (domain + endpoint/integration).

## Confirmed scope decisions
- Minimal v1 uses HTTP endpoints first for send/receive.
- SignalR is deferred to the next increment.
- Auth is deferred for minimal v1 (OIDC/Auth0 boundary prepared later).
- Initial bounded context is `Chat`.

## Minimal scope (v1)
- `POST /api/messages` -> validate and persist a message.
- `GET /api/messages?roomId=...` -> return messages for a room/page key.
- `GET /health` -> liveness endpoint.
- PostgreSQL persistence through Dapper.
- DbUp migration for initial schema (`messages` table).

## Proposed DDD + slice structure
- `src/Backend.Api`
  - Composition root, DI, endpoint registration.
- `src/Backend.Domain/Chat`
  - Entities/value objects and invariants.
- `src/Backend.Infrastructure/Persistence`
  - Dapper repositories, connection factory, DbUp runner.
- `src/Backend.Features/Chat/SendMessage`
  - Request model, validation, handler, endpoint mapping.
- `src/Backend.Features/Chat/GetMessages`
  - Query model, handler, endpoint mapping.

## Testing strategy (from the start)
- `tests/Backend.Domain.Tests`
  - Domain invariants and rule enforcement.
- `tests/Backend.Api.Tests`
  - Endpoint contract tests and validation paths.
  - Integration tests using PostgreSQL container + applied DbUp migration.
- CI baseline:
  - Run `dotnet test` for backend test projects on pull requests.

## Commit-sized implementation tasks

### Task 1: Scaffold backend solution and projects in slice-oriented layout
Repository starts from docs only, so this task establishes the execution baseline for all following slices and tests.

- [ ] Create `.NET 8` solution with projects for `Backend.Api`, `Backend.Domain`, `Backend.Infrastructure`, and feature slices.
- [ ] Wire project references and baseline dependency injection composition in `Backend.Api`.
- **Commit**: `scaffold backend solution and projects`

### Task 2: Implement `Chat` domain model and invariants
This task anchors DDD-first behavior and ensures core message rules are enforced before persistence and API work.

- [ ] Add `Chat` domain entities/value objects for messages and enforce invariants (required fields, bounds, timestamp rules).
- [ ] Add domain tests for both valid message creation and invariant/validation failure paths.
- **Commit**: `implement chat domain model and invariants`

### Task 3: Create initial DbUp migration and Dapper repository for messages
Persistence should follow the selected stack (`PostgreSQL + DbUp + Dapper`) and support room/page-based message retrieval.

- [ ] Create initial DbUp migration for the `messages` table and room-oriented query index(es).
- [ ] Implement Dapper-based repository and connection factory in infrastructure.
- **Commit**: `add dbup migration and dapper message repository`

### Task 4: Implement minimal send/receive endpoints in separate feature slices
This delivers the minimal v1 HTTP contract while preserving vertical slice boundaries.

- [ ] Implement `POST /api/messages` slice with request model, validation, handler, and persistence call.
- [ ] Implement `GET /api/messages?roomId=...` slice and register `GET /health` liveness endpoint.
- **Commit**: `implement minimal message send receive endpoints`

### Task 5: Add domain and integration tests for all minimal endpoints
Tests are required from the start; this task verifies endpoint contracts and infrastructure wiring against PostgreSQL.

- [ ] Add API tests for success and validation/error responses on `POST /api/messages` and `GET /api/messages`.
- [ ] Add PostgreSQL-backed integration tests that apply DbUp migrations before running endpoint scenarios.
- **Commit**: `add domain and api integration tests`

### Task 6: Add minimal backend run/test documentation to `README.md`
The final task ensures first-time contributors can run and verify the backend without extra tribal knowledge.

- [ ] Update `README.md` with backend startup flow and required environment variable configuration.
- [ ] Add backend test execution instructions and local PostgreSQL expectations.
- **Commit**: `document backend run and test workflow`

## Notes and guardrails
- Keep contracts and payloads minimal for fast iteration.
- Prefer explicit error responses for validation failures.
- No queues, Redis, or background workers in this first implementation.
- Keep SignalR and auth as next slices after HTTP baseline is stable.
