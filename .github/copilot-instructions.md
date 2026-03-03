# solita-chat-demo Copilot instructions

## Git

- All Git worktrees must be created under the repository root `.worktrees/` directory.

## Executing Plans

When executing a plan from `.github/docs/plans/`, follow these rules:

- **One task at a time.** Finish current task before starting next.
- **Mark progress.** Set finished task checkboxes to `[x]` in the plan file.
- **Commit after each task.** If task changed files, stage and commit with the task's **<Title>**:
   ```
   git add -A
   git commit -m "<title of the task>"
   ```
- **Build before committing.** Run task build command; else use this file's **Build and Test** commands.
- **Stop on failure.** If any step fails, fix it before moving to the next task.
- **Document deviations.** If you must deviate from the plan, update it with what changed and why the original approach could not be followed.
