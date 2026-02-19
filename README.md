# solita-chat-demo
Between projects work: A chrome extension where every page you are on is a chat room

## AI prompt logging (Copilot hooks)

- Hook config: `.github\hooks\prompt-log.json`
- Log path config: `.github\hooks\prompt-log-path.txt`
- Hook scripts: `scripts/prompt-log.sh` and `scripts/prompt-log.ps1`
- Log format: newest entry first, with `YYYY-MM-DD HH:mm:ss` on its own line followed by the original prompt lines.
