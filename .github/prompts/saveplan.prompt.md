---
agent: 'agent'
description: 'Save the current session plan under the repo'
---

1. Take the current session plan (`plan.md` from the session folder) and save it under `.github/docs/plans/` as is. Perserve all the details. The document should be self contained so a fresh agent with clear context should be able to implement the changes based on the plan.
2. Name the plan to `<YYYY-MM-DD>_<kebab-case summary derived from the plan>.md`.
3. Reorganize the todo section so each step is commit sized task. 
   - Add checkboxes to the task items.
   - Add short commit message for the task.
   - See example template to follow:
   ```
   Task N: <Title>
   - [ ] Actionable step
   - [ ] Actionable step
   - **Commit**: `<short imperative commit message>`
   ```