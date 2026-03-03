---
agent: 'agent'
description: 'Save the current session plan under the repo'
---

1. Take the current session plan.
2. Condense it and fit it into the template at `.github/docs/plans/template.md`. The saved plan should be self-contained with only enough detail so a fresh agent with clear context should be able to implement the changes based only on the plan.
3. Divide the implementation steps into commit sized tasks.
2. Save the result to `.github/docs/plans/<YYYY-MM-DD>_<kebab-case summary derived from the plan>.md`.
