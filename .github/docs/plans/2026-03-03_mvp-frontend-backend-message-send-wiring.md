# Implementation Plan: MVP frontend-backend message flow wiring

## Problem
Frontend popup chat currently uses local mock data and does not call the backend for listing or sending messages.  
Backend already supports `GET /api/messages` (room-scoped paged messages) and `POST /api/messages` (message creation with `roomId`, `authorName`, `text`).  
For MVP, frontend should load messages from backend and send new messages using hardcoded author and room id, with minimal UI changes.

## Proposed approach
Keep the current popup layout/components, and wire the core message flow to backend:
- Add a tiny frontend API module for both `GET /api/messages` and `POST /api/messages`.
- Use frontend constants for backend URL, author, room id, and initial page size.
- Update `App.vue` to load room messages on popup open and render backend results.
- Update `App.vue` send handler to post messages and keep the message list in sync.
- Add minimal loading/error feedback in the popup.
- Ensure extension manifest allows backend calls from popup.

## Tasks
1. **Create frontend API client for message fetch/send**
- [ ] Add `src/Frontend/src/api/chat-api.ts` with typed request/response models for `GET /api/messages` and `POST /api/messages`.
- [ ] Add MVP constants in frontend (backend base URL, fixed `roomId`, fixed `authorName`, initial `pageSize`) and use them in API calls.
- [ ] Expose `getMessages` and `sendMessage` helpers for popup use.
- **Commit**: `feat(frontend): add mvp chat api client for get and send`

2. **Wire initial message loading in popup app**
- [ ] Update `src/Frontend/src/App.vue` to load messages on mount using `getMessages` for the fixed room id.
- [ ] Map `GET /api/messages` response items to the existing `ChatMessage` UI model and keep current bubble rendering.
- [ ] Add minimal user-visible loading/error state for fetch failures while preserving existing UI structure.
- **Commit**: `feat(frontend): load popup messages from backend`

3. **Wire composer send flow in popup app**
- [ ] Update `src/Frontend/src/App.vue` `handleSend` to async-call `sendMessage` instead of only local push.
- [ ] Map backend response fields (`id`, `authorName`, `text`) to `ChatMessage` and keep list ordering consistent with loaded messages.
- [ ] Add minimal user-visible error state for send failures while preserving existing UI structure.
- **Commit**: `feat(frontend): wire popup send action to backend`

4. **Allow extension popup to reach backend**
- [ ] Update `src/Frontend/public/manifest.json` host permissions for local backend origin used by MVP (for example `http://localhost:5027/*`).
- [ ] Keep permission scope minimal to required MVP backend origin(s).
- **Commit**: `chore(extension): allow popup backend host access`

5. **Validate MVP integration path**
- [ ] Run frontend validation (`npm run type-check` and `npm run build` in `src/Frontend`).
- [ ] Run backend locally and verify popup load fetches from `GET /api/messages` and send creates message through `POST /api/messages`.
- [ ] Update README only if needed to document MVP frontend constants/config assumptions.
- **Commit**: `chore: validate mvp frontend-backend message flow`

## Decisions / notes / assumptions
- Frontend uses hardcoded values for now:
  - `authorName`: fixed constant
  - `roomId`: fixed constant
- MVP loads one initial page of messages; pagination UX is out of scope.
- Backend contract remains unchanged; no API/backend feature work is required for this MVP.
