---
agent: 'agent'
description: 'Save the current session plan under the repo'
---

1. Take the current session plan (`plan.md` from the session folder) and save it under `.github/docs/plans/` as-is. Preserve all the details. The saved plan should be self-contained so a fresh agent with clear context should be able to implement the changes based on the plan.
2. Name the plan to `<YYYY-MM-DD>_<kebab-case summary derived from the plan>.md`.
3. Reorganize the todo section so each step is a balanced commit-sized task.
   - Merge tiny related steps into one task so commits are meaningful (avoid one-file commits unless truly isolated).
   - Keep each task focused on one coherent change; split tasks that mix unrelated concerns.
   - Aim for medium scope: usually one feature slice touching a few related files (roughly 2-6 files), not a huge refactor.
   - Add checkboxes to the task items.
   - See example template to follow:
   ```
   1. **<Title>**
   - [ ] step
   - [ ] step
   ```
