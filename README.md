# solita-chat-demo
Between projects work: A chrome extension where every page you are on is a chat room

## AI prompt logging (Copilot hooks)

This repo logs every submitted Copilot prompt to `ai\prompts\solita-markump-prompts.md` via `.github\hooks\prompt-log.json`.
Each line format is: `<timestamp> > <prompt>`.

Quick checks:
- Latest entries: `Get-Content .\ai\prompts\solita-markump-prompts.md -Tail 20`
- Full log: `Get-Content .\ai\prompts\solita-markump-prompts.md`
