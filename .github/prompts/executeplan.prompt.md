---
description: Execute a plan file step by step
agent: agent
---

# Execute Plan

Execute the plan at `${{ input:planPath }}`.

When executing the plan, follow these rules:

- **One task at a time.** Finish current task before starting next.
- **Mark progress.** Set finished task checkboxes to `[x]` in the plan file.
- **Commit after each task.** Take the **Commit** section and use it as a message.
   ```
   git add -A
   git commit -m "<commit message from the plan's task>"
   ```
- **Document deviations.** If you must deviate from the plan, update the plan file with what changed and why the original approach could not be followed.
- **Mark the plan as complete.** Once all the tasks are marked complete -> add completed timestamp after the title of the plan.
