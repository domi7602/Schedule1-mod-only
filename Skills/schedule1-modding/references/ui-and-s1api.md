# UI integration and framework choice

> verified: S1API section checked against 2026-10-05 (icon policy + clamp curves); Method 3 section since 2026-08-20.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 27 of 27 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Before implementation

Confirm whether the feature belongs in a PhoneApp, HUD, world-space UI or existing native screen. Inspect the matching current base class and project UI helpers; do not infer override accessibility or registration APIs from an old skeleton.

## Choose the relevant contract

| Concern | Canonical reference |
|---|---|
| Close/reopen, container ownership, subscriptions, independent model state | [PhoneApp lifecycle](../../schedule1-phoneapp/references/lifecycle-and-canvas.md) |
| Add/update/remove rows, stable selection/scroll, control rebinding | [Components and reconciliation](../../schedule1-phoneapp/references/components-and-ugui.md) |
| Typing and modal ownership | [Input arbitration](../../schedule1-phoneapp/references/input-focus-and-controls.md) |
| Shared UITheme, measured rects, display/scale/long-text checks | [Responsive layout](../../schedule1-phoneapp/references/responsive-ui-theme.md) |
| Framework members, icon lookup and owned sprite/texture lifetime | [S1API PhoneApp contract](../../schedule1-s1api/references/phoneapp.md) |

Confirm resource deployment in the current project. Missing icons need a bounded
diagnostic and supported fallback, not repeated allocation.

## Commands

If a diagnostic console command is needed, inspect the actual S1API BaseConsoleCommand definition and working project examples. Do not invent registration attributes, method names or parameter types. Keep read-only status separate from state-changing commands and report actual readback.

## Acceptance

Use the selected reference's checks; report compile and gameplay evidence separately.
