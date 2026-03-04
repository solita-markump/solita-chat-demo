# Implementation Plan: Wire Frontend to Backend API (MVP)

## Problem
The Vue 3 frontend uses hardcoded mock data (`mock-data.ts`). It needs to call the real backend REST API (`GET /api/messages`, `POST /api/messages`) with a hardcoded author (`Markus`) and room (`general`), and poll for new messages every 5 seconds.

## Proposed approach
Add CORS to the backend, create a frontend API client, replace mock data usage in components with real fetch/post calls and a 5-second polling interval, then delete the mock-data files.

## Tasks

1. **Add CORS middleware to backend**
   - [ ] In `src/Backend.Api/Program.cs`, register a CORS policy allowing origins `http://localhost:5173` (Vite dev) with any method/header
   - [ ] Add `app.UseCors()` before `app.MapBackendEndpoints()`
   - **Commit**: `feat(api): add CORS policy for local dev origins`

2. **Extract ChatMessage type and create API client**
   - [ ] Create `src/Frontend/src/types.ts` with the `ChatMessage` interface (moved from `mock-data.ts`)
   - [ ] Create `src/Frontend/src/api.ts` with `fetchMessages(roomId)` → `GET /api/messages?roomId=` and `sendMessage(roomId, authorName, text)` → `POST /api/messages`. Map API response to `ChatMessage`, derive `isMine` by comparing `authorName === 'Markus'`
   - [ ] API base URL: `http://localhost:5027`
   - **Commit**: `feat(frontend): add API client and ChatMessage types`

3. **Wire App.vue and update component imports**
   - [ ] Update `src/Frontend/src/App.vue`: import from `api.ts`, fetch messages `onMounted`, POST on send, set up 5s polling with `setInterval` / `onUnmounted` cleanup. Hardcode `roomId = 'general'` and `authorName = 'Markus'`
   - [ ] Update `src/Frontend/src/components/ChatBubble.vue`: change `ChatMessage` import from `@/mock-data` to `@/types`
   - **Commit**: `feat(frontend): wire chat UI to backend API with polling`

4. **Delete mock-data files**
   - [ ] Remove `src/Frontend/src/mock-data.ts`, `src/Frontend/src/mock-data.js`, `src/Frontend/src/mock-data.js.map`
   - **Commit**: `chore(frontend): remove mock-data files`

## Decisions / notes / assumptions
- No error handling beyond `console.warn` — MVP only
- Polling via `setInterval`; SignalR will replace this later
- CORS over Vite proxy since Chrome extension will need CORS anyway
