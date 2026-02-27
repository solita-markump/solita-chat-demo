---
agent: 'agent'
description: 'Save the current session plan under the repo'
---

1. Read the current session plan (`plan.md` from the session folder).
2. Preserve all context, rationale, technical details, and notes from the plan — a fresh agent with no prior context must be able to implement the feature from this document alone.
3. Group the implementation steps into commit-sized tasks. Convert only the actionable steps into checkboxes. Keep all surrounding detail, explanations, and context as-is. Use this structure for each task:
   ```
   ### Task N: <Title>
   <any relevant context, rationale, or technical notes for this task>

   - [ ] Actionable step
   - [ ] Actionable step
   - **Commit**: `<short imperative commit message>`
   ```
4. Save the result to `docs/plans/<YYYY-MM-DD>-<name>.md` (create the directory if needed). `<name>` is a short kebab-case summary derived from the plan title.