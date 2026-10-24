# Plan changes at the appropriate scope

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 5 of 5 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Choose the task mode

- Review: inspect and explain; do not silently implement.
- Plan: produce a concrete implementation plan and acceptance checks.
- Implement: execute the requested change and its authorized verification.
- Configured maker/checker: use the actual project's reviewer and approval requirement when present.

An explicit implementation request is not automatically converted into a second permission request. Conversely, a bug report alone is not a request to deploy, publish or rewrite unrelated systems. Resolve mode from the current task and repository instructions. Do not invent @gatekeeper, @designer or @coder roles when they do not exist.

## Minimum plan for risky integration changes

Before editing lifecycle, persistent data, payments, native detours or networked state, state:

1. Problem and observed evidence.
2. Intended behavior and numbered acceptance criteria.
3. Owners and dependencies.
4. Relevant transitions and failure/partial-effect cases.
5. Proposed implementation boundary and order.
6. Verification that discriminates the fix from the old failure.
7. Missing evidence and how it affects only the dependent work.

For a small isolated change this can be a short working note. Do not create a multi-stage bureaucracy for a spelling correction.

## Implementation

Inspect existing changes, preserve unrelated work and implement within the agreed scope. If new evidence invalidates the plan, explain the material change and adjust. Ask only when the remaining choice changes authorization, the user's outcome or an important unresolved requirement.

## Review and delivery

Show what changed and why, actual checks and results, residual risks and any unexecuted manual step. Do not imply that a plan, source inspection or build is gameplay acceptance.

## Historical context

The uploaded collection recorded a group-chat maker/checker convention from 2026-08-27. That history explains the old wording but does not establish that every future Hermes session has those roles or requires another approval.
