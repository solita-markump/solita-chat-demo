---
agent: 'agent'
description: 'Save the current session plan under the repo'
---

1. Read the current session plan (`plan.md` from the session folder).
2. Restructure the implementation steps into commit-sized tasks. Use this template for each task:
   ```
   ### Task N: <Title>
   - [ ] Subtask
   - [ ] Subtask
   - **Commit**: `<short imperative commit message>`
   ```
3. Save the result to `docs/plans/<YYYY-MM-DD>-<name>.md`. `<name>` is a short kebab-case summary derived from the plan title.