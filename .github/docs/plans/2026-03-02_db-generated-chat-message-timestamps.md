# Implementation Plan: DB-generated chat message timestamps

## Problem
- `ChatMessage.Create(...)` currently requires `createdAtUtc` as an input parameter.
- `POST /api/messages` accepts and validates `createdAtUtc` from clients.
- `DapperChatMessageRepository.SaveAsync` inserts the provided value into `messages.created_at_utc`.
- Target behavior is to generate `created_at_utc` when the row is inserted into the database.

## Proposed approach
- Remove timestamp input from message creation and request validation path.
- Add a DB default for `messages.created_at_utc` via a new migration script (without rewriting existing migration files).
- Update repository insert to omit `created_at_utc` from input values and fetch the generated value with `RETURNING`.
- Keep `ChatMessage.CreatedAtUtc` on persisted entities by returning a saved/rehydrated message from repository save.
- Update API response construction and tests to use DB-generated timestamps.

## Todos
1. **Adjust domain creation model**
   - [x] Update `ChatMessage.Create(...)` to stop accepting external timestamp.
   - [x] Remove timestamp validation that is only relevant to caller-provided values.
   - [x] Keep `Rehydrate(...)` and UTC checks for DB-loaded data.

2. **Update persistence contract and implementation**
   - [x] Change `IChatMessageRepository.SaveAsync` to return the persisted `ChatMessage` (or equivalent carrying generated timestamp).
   - [x] Update `DapperChatMessageRepository.SaveAsync` to insert without `created_at_utc` parameter.
   - [x] Use `RETURNING created_at_utc`.
   - [x] Return a message instance containing the DB-generated timestamp.

3. **Add migration for DB timestamp default**
   - [x] Create a new embedded SQL migration script (next numeric prefix) to set a default on `messages.created_at_utc`.
   - [x] Preserve compatibility for existing rows and schema.

4. **Update send-message feature surface**
   - [x] Remove `CreatedAtUtc` from `SendMessageRequest`.
   - [x] Remove endpoint validation for `createdAtUtc`.
   - [x] Update `SendMessageHandler` to use the new domain/repository flow.
   - [x] Keep or adjust `SendMessageResponse.CreatedAtUtc` based on API contract decision.

5. **Update tests**
   - [x] Update domain tests that currently depend on caller-supplied timestamps.
   - [x] Update API endpoint tests to stop sending `createdAtUtc` in POST payloads.
   - [x] Verify POST response timestamp behavior and GET ordering/pagination assumptions.

6. **Validate changes**
   - [ ] Run existing build and test suites for backend/domain/api projects to confirm no regressions.

## Decision
- `POST /api/messages` will continue returning `createdAtUtc` immediately by reading the DB-generated value after insert.
