# Implementation Plan: frontend Vue + Tailwind extension popup draft

## Problem
- Repository source is currently backend-only (`src\Backend.*`, `tests\Backend.*`) with no extension/frontend workspace.
- Product direction needs a Chrome extension popup UI, but no `manifest.json`, frontend build tooling, or popup UI exists yet.
- For this increment, we need a minimal popup draft only, intentionally without backend integration.

## Proposed approach
1. Scaffold a Vue 3 + TypeScript workspace at `src\Frontend`.
2. Add Chrome Extension Manifest V3 popup wiring (`action.default_popup`) and build output suitable for load-unpacked.
3. Configure Tailwind CSS for popup styling.
4. Implement a static chat-shell popup UI with mock/local data only.
5. Document setup/build/load flow and validate with available checks.

## Tasks
1. **Scaffold Vue extension workspace**
- [ ] Initialize npm-based Vue + TypeScript project under `src\Frontend`.
- [ ] Add Manifest V3 config and popup entry wiring so clicking the extension icon opens the popup.
- **Commit**: `scaffold vue extension workspace`

2. **Configure Tailwind for popup**
- [ ] Install and configure Tailwind + PostCSS in the frontend project.
- [ ] Add popup base size/layout styles for a consistent extension popup frame.
- **Commit**: `configure tailwind for extension popup`

3. **Build minimal popup chat draft**
- [ ] Implement popup UI sections: header/title, mock message list, and composer (input + send button).
- [ ] Keep functionality static (no API calls, auth, persistence, or realtime transport).
- **Commit**: `build minimal popup chat ui draft`

4. **Document frontend workflow**
- [ ] Update `README.md` with install/build and Chrome load-unpacked instructions for the popup extension.
- [ ] Update `architecture.md` frontend stack to Vue + Tailwind.
- **Commit**: `document frontend extension workflow`

5. **Validate draft**
- [ ] Run frontend checks available from the scaffold (`build`, `type-check`, `lint` if present).
- [ ] Run relevant existing backend checks to ensure no regressions from repo changes.
- **Commit**: `validate frontend draft and backend stability`

## Decisions / notes / assumptions
- Popup v1 scope is fixed to a static chat shell only (header + mock messages + composer).
- Frontend project location is fixed to `src\Frontend`.
- TypeScript is the chosen language; npm is the default package manager.
- Out of scope for this increment: backend integration, URL-to-room mapping, authentication, and SignalR/realtime behavior.
