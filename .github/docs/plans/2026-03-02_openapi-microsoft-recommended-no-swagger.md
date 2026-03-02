# Implementation Plan: OpenAPI docs (Microsoft-recommended, no Swagger)

## Problem
- The backend API does not expose an OpenAPI document endpoint.
- The backend API does not expose an interactive API reference UI.
- The target is the Microsoft-recommended approach for ASP.NET Core Minimal APIs, explicitly without Swashbuckle/Swagger.

## Proposed approach
- Use built-in OpenAPI generation with `Microsoft.AspNetCore.OpenApi`.
- Expose OpenAPI JSON via `MapOpenApi()` in Development only.
- Expose interactive API docs with `Scalar.AspNetCore` via `MapScalarApiReference()` in Development only.
- Keep existing message API behavior unchanged.
- Enrich endpoint metadata so generated docs are clearer.
- Update `README.md` with local usage guidance.

## Commit-sized tasks
1. **Add OpenAPI and Scalar dependencies**
   - [x] Add `Microsoft.AspNetCore.OpenApi` to `src\Backend.Api\Backend.Api.csproj`.
   - [x] Add `Scalar.AspNetCore` to `src\Backend.Api\Backend.Api.csproj`.
   - [x] Ensure package restore succeeds.
   - **Commit**: `add openapi and scalar packages`

2. **Wire OpenAPI services and development-only routes**
   - [x] Register `builder.Services.AddOpenApi()` in `src\Backend.Api\Program.cs`.
   - [x] Map `app.MapOpenApi()` only in Development.
   - [x] Map `app.MapScalarApiReference()` only in Development.
   - [x] Keep existing startup/migration behavior intact.
   - **Commit**: `wire built-in openapi and scalar routes`

3. **Annotate chat minimal API endpoints for OpenAPI quality**
   - [ ] Add tags/summary/description and response metadata to `src\Backend.Features\Chat\SendMessage\SendMessageEndpoint.cs`.
   - [ ] Add tags/summary/description and response metadata to `src\Backend.Features\Chat\GetMessages\GetMessagesEndpoint.cs`.
   - [ ] Keep validation and runtime behavior unchanged.
   - **Commit**: `annotate chat endpoints for openapi docs`

4. **Document local OpenAPI usage**
   - [ ] Update `README.md` with OpenAPI JSON route.
   - [ ] Update `README.md` with Scalar UI route.
   - [ ] Note that documentation endpoints are Development-only.
   - **Commit**: `document openapi and scalar usage`

5. **Verify and close out**
   - [ ] Run existing tests with `dotnet test .\SolitaChatDemo.sln -v minimal`.
   - [ ] Confirm no regressions.
   - [ ] Mark completed tasks in this plan.
   - **Commit**: `verify openapi documentation integration`

## Notes
- Do not add Swashbuckle/Swagger packages.
- Keep docs endpoints disabled outside Development.
- Keep changes minimal and focused on documentation exposure.
