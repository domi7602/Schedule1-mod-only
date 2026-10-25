# Plan changes at the appropriate scope

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 5 of 5 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Use the [working contract](working-contract.md) for task mode, authorization and preservation of unrelated work. This reference adds planning detail for risky integrations; a plan request itself does not authorize implementation.

## Minimum plan for risky integration changes

For lifecycle, persistence, payments, native detours or networked state, state:

1. Problem/evidence and intended behavior.
2. Acceptance criteria, owners and dependencies.
3. Transitions, partial/unknown effects and recovery.
4. Implementation boundary and order.
5. Checks that distinguish the fix from the old failure, including missing prerequisites.

Scale this to the change: a small isolated fix needs only a short note, not a separate planning document.

Adjust the plan when evidence changes it. Ask only for unresolved choices that affect authorization, outcome or requirements. Report actual checks and limits using the [evidence policy](evidence-policy.md).

## Historical context

The uploaded collection recorded a group-chat maker/checker convention from 2026-08-27. That history explains the old wording but does not establish that every future Hermes session has those roles or requires another approval.
