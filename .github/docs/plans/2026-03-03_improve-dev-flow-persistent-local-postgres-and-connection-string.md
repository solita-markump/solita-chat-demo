# Dev flow improvement plan

## Current state analysis
- README uses an ad-hoc `docker run` command to start PostgreSQL locally.
- The Docker setup is not codified as reusable project configuration (no checked-in compose file).
- Backend startup currently expects `DATABASE_CONNECTION_STRING` env var each shell session, though code already supports `ConnectionStrings:Chat` fallback.
- `appsettings.Development.json` currently has no local database connection string.

## Goal
Make local development startup repeatable and persistent so developers do not need to recreate database and re-export connection string every run.

## Proposed approach
1. Add a committed Docker Compose setup for local PostgreSQL with a named volume and stable container/service config.
2. Add a persistent development connection string source so local runs work without setting env vars every time.
3. Update README to document the new one-time and daily dev flow commands.
4. Verify the updated flow by running backend and relevant tests.

## Todos
1. **Audit current dev-flow behavior**
- [x] Confirm README and `Backend.Api` configuration behavior for Docker startup and connection-string resolution.
- [x] Ensure planned changes keep production behavior unchanged and only improve local development ergonomics.

2. **Add persistent local PostgreSQL setup**
- [x] Add a checked-in Docker Compose file for local PostgreSQL with fixed port mapping and env defaults.
- [x] Configure a named volume so database state persists across restarts.

3. **Persist local development connection configuration**
- [ ] Add `ConnectionStrings:Chat` to `src\Backend.Api\appsettings.Development.json` for local runs.
- [ ] Keep environment-variable override behavior (`DATABASE_CONNECTION_STRING`) intact.

4. **Update developer instructions**
- [ ] Replace ad-hoc `docker run` and per-shell env export steps in `README.md` with repeatable `docker compose up -d` flow.
- [ ] Document where the local connection string is configured and how to override it when needed.

5. **Validate updated dev flow**
- [ ] Run the backend with the updated local setup to verify migrations and startup behavior.
- [ ] Run relevant tests/build commands to confirm changes are working as documented.

## Notes and confirmed decisions
- Use `appsettings.Development.json` + `ConnectionStrings:Chat` as the persistent local development connection-string strategy.
- Keep production behavior unchanged; only improve local development ergonomics.
