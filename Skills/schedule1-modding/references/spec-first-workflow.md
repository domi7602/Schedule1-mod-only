# Spec-First Workflow (Maker-Checker Convention)
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 5 of 5 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.


> Process convention (not game-API knowledge) — extracted from `schedule1-modding` §2.D 2026-10-05 to keep the runbook skill code-focused. Verified convention 2026-08-27.

**Rule (2026-08-27, group-chat convention with `@gatekeeper` + `@designer`):** Never start a code change just because a user reported a problem. The maker-checker chain (`@coder` writes, `@gatekeeper` reviews) requires an explicit green light after a Spec, BEFORE any `dotnet build`.

## When This Applies

- Bug fixes, refactors, new features touching >1 file
- Any change to a hot path (Harmony prefix/postfix, Update loops, polling)
- Any change to save/load lifecycle hooks (`OnPreLoad`, `OnLoadComplete`, `OnSaveComplete` — note: `OnSaveInfoLoaded` fires 0× in the instrumented session, verified 2026-09-29; treat changes to subscriptions of it as dead-code removals needing a Spec too)

## Spec Template (paste into the chat before any code touches a file)

1. **Trigger / problem** — what's broken, where, repro
2. **Spec items as a numbered checklist** — concrete acceptance criteria, NOT aspirational
3. **Pitfalls / edge cases** — list each one with the chosen handling (e.g. "What if X happens during a scene reload? — ResetState() before spawn cycle")
4. **Logging / observability** — what counters or log lines prove the fix fires
5. **Build order** — "first A (critical), then B/C/D"
6. **Open questions** — anything ambiguous; ask before coding, not during

## Workflow

1. User reports problem → coder reads code, finds root cause
2. Coder drafts Spec (template above) in the chat → gatekeeper reviews, pokes holes, suggests edge cases
3. Spec gets explicit "green" / "spec ok, build" → coder builds in the order listed
4. Build green → coder reports what changed, where, what to test → gatekeeper does the review

## Don't Do

- Don't ask "may I build?" — write the Spec, wait for green light, then build
- Don't skip the Spec "just because it's a small fix" — small fixes have the biggest hidden regressions
- Don't list build-order bullets that aren't in the chat (e.g. "I'll also fix X while I'm in there" — out of scope; surface it as a separate Spec if needed)

## Live References

CustomSkateboard `IsInstanceTuned` early-out fix (Spec → green → build green) and HomelessMod F-key + slot-switch fix (Spec → 3 answer round → green with edge-case list → build green) both ran this way on 2026-08-27 and shipped without a revert.
