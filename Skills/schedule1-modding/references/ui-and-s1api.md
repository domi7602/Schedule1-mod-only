# UI integration and framework choice

> verified: S1API section checked against 2026-10-05 (icon policy + clamp curves); Method 3 section since 2026-08-20.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 27 of 27 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Before implementation

Confirm whether the feature belongs in a PhoneApp, HUD, world-space UI or existing native screen. Inspect the matching current base class and project UI helpers; do not infer override accessibility or registration APIs from an old skeleton.

Use [PhoneApp](../../schedule1-phoneapp/SKILL.md) for lifecycle, input and layout. Use [S1API](../../schedule1-s1api/SKILL.md) to locate the actual public interface.

## Presentation contract

- Keep domain state independent from concrete UI objects.
- Reconcile added, changed and removed rows, not only visible additions.
- Rebuild event bindings for the actual control instance and dispose final subscriptions owned by that instance.
- Distinguish close/hide from destruction. Reopen must remain functional.
- Acquire and release typing/focus through a shared owner-aware coordinator.
- Preserve selection and scroll position by stable domain identity when still valid.

## Scaling

Reuse the actual Shared UITheme where present. The uploaded collection described text and dashboard profiles with different scaling curves; inspect their current implementation rather than copying constants into every mod. Define which rect is measured and when layout has settled.

Validate narrow phone content, long text, supported resolutions, user scale and repeated rebuild. A clamp alone does not prevent overflow or guarantee legibility.

## Assets

Confirm how icons/resources are resolved and deployed by the current framework and project. Give runtime-generated sprites/textures explicit ownership and cleanup. A missing icon should produce a bounded diagnostic and a supported fallback, not repeated allocation.

## Commands

If a diagnostic console command is needed, inspect the actual S1API BaseConsoleCommand definition and working project examples. Do not invent registration attributes, method names or parameter types. Keep read-only status separate from state-changing commands and report actual readback.

## Acceptance

Verify create, first open, close/reopen, rebuild, scene transition, input focus, removed data and final teardown. Report compile and gameplay checks separately.
