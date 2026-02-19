# solita-chat-demo
Between projects work: A chrome extension where every page you are on is a chat room

## AI prompt logging (Copilot hooks)

This repo logs every submitted Copilot prompt payload to `ai\prompts\prompt-log.jsonl` via `.github\hooks\prompt-log.json`.

Quick checks:
- Latest entries: `Get-Content .\ai\prompts\prompt-log.jsonl -Tail 20`
- Full log: `Get-Content .\ai\prompts\prompt-log.jsonl`
