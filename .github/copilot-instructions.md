# solita-chat-demo Copilot instructions

## Git

- All Git worktrees must be created under the repository root `.worktrees/` directory.

## Planning

When creating plans, structure them so a fresh agent with no prior context can implement the feature from the document alone:

- **Commit-sized tasks.** Group implementation steps into tasks where each task = one commit.
- **Preserve context.** Keep all rationale, technical details, and notes — don't strip them down to bare checkboxes.
- **Task structure:**
   ```
   ### Task N: <Title>
   <any relevant context, rationale, or technical notes for this task>

   - [ ] Actionable step
   - [ ] Actionable step
   - **Commit**: `<short imperative commit message>`
   ```

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
