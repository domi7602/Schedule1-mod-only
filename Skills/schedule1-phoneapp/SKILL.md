---
name: schedule1-phoneapp
description: "Build Schedule I phone UI with correct focus and lifetime."
---

# Schedule I — Phone Apps

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Use this skill for the app layer; load economy or persistence only when the task changes those responsibilities.

## Establish the target and boundary

Resolve the actual S1API PhoneApp base, framework assembly, game branch and Shared helpers
from current `AGENTS.md` / `docs/compatibility.md`. Inspect discovery, canvas and input
setup before changing controls. Historical notes do not certify the installed runtime.

- Let verified discovery own app instantiation; create one app-owned root under its container.
- Separate close/hide from destruction; retain visibility sync and framework close bookkeeping.
- Bind delegates to their actual owner lifetime, use the verified IL2CPP adapter, and reject
  stale scene/owner callbacks before accessing Unity objects. A new instance cannot unsubscribe
  the old instance's handler.
- Release only this app's input, camera, theme and listener ownership; another UI's focus lock
  must survive. Keep save-bound history/progress separate from global preferences.
- On failed state loading, preserve the write gate and recoverable data. Missing resources/helpers
  need a bounded diagnostic and a disabled affected feature or the last usable UI.

## Load for the changed behavior

| Trigger | Contract and focused checks |
|---|---|
| Registration, ownership, orientation, blank/reopened app, reload or assets | [Lifecycle and canvas](references/lifecycle-and-canvas.md) |
| Layout, Shared UITheme metrics, mockup/font parity, dark theming or hover clipping | [Responsive UI](references/responsive-ui-theme.md) |
| Typing, shortcuts, focus transfer, gamepad/mouse mismatch | [Input focus and controls](references/input-focus-and-controls.md) |
| Buttons, stable-ID lists, removed/reordered rows, scrolling, modals or inline pickers | [Components and uGUI](references/components-and-ugui.md) |

Choose acceptance checks from the affected reference, including reopening and any changed
focus/data/slot behavior. Report observed UI, focus and lifetime results under the shared
evidence policy; keep historical incidents scoped to their recorded session.
