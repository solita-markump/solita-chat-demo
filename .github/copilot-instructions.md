# solita-chat-demo Copilot instructions

## Executing Plans

When executing a plan from `docs/plans/`, follow these rules:

- **One task at a time.** Finish current task before starting next. One task = one commit.
- **Mark progress.** Set finished task checkboxes to `[x]` in the plan file.
- **Commit after each task.** If task changed files, stage and commit with the task's **Commit** message:
   ```
   git add -A
   git commit -m "<commit message from task>"
   ```
- **Build before committing.** Run task build command; else use this file's **Build and Test** commands.
- **Stop on failure.** Fix failing step before continuing.