# solita-chat-demo
Between projects work: A chrome extension where every page you are on is a chat room

## AI prompt logging (Copilot hooks)

- Hook config: `.github\hooks\prompt-log.json`
- Log username config: `.github\hooks\prompt-log-username` (single-value file, no extension; e.g. `solita-markump`; logs go to `ai\prompts\<username>\<branch>.log`)
- Hook script: `scripts\prompt-log.ps1`
- Log format: newest entry first, with `YYYY-MM-DD HH:mm:ss` on its own line followed by the original prompt lines.
- Log files under `ai\prompts\` are intentionally tracked in git; do not add them to `.gitignore`.
