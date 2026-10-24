---
name: schedule1-phoneapp
description: "Build Schedule I phone UI with correct focus and lifetime. Keywords: PhoneApp, S1API, uGUI, UITheme, Sp, Dp, Method 3, responsive, InputFocus, IsTyping, OnCreated, OnPhoneClosed, IsOpen, UIFactory, ButtonUtils, EventHelper, slot isolation, IconSprite, mockup, restyle."
---

# Schedule I — Phone Apps

Apply the [shared working contract](../schedule1-modding/references/working-contract.md).
Use this skill for the app layer; load economy or persistence only when the task changes those responsibilities.

## Establish the target

1. Identify the actual PhoneApp base, framework assembly, game branch, and relevant shared helpers.
2. Read current `AGENTS.md` / `docs/compatibility.md` when present. Treat S1API 3.2.1 as the requested target, not as verified by this package. <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->
3. Verify discovery, instance lifetime, UI callbacks, close behavior, and permanent teardown against that assembly.
4. Inspect the existing canvas and input setup before choosing a layout or replacing a control.
5. Record acceptance criteria: reopening, typing, navigation, data refresh, and any slot-specific state.

## Build or repair the app

1. Let the verified S1API discovery path own app instantiation; do not create a competing instance.
2. Declare the intended orientation explicitly using the supported override.
3. Create one owned root under the framework container. Keep framework-owned ancestors intact.
4. Build reusable widgets once. Synchronize the owned root with the actual open state.
5. Keep ordinary phone close separate from instance destruction. Hide/reset reusable UI on close; detach handlers and release owned assets at final teardown.
6. Register each delegate for one owner lifetime. Removing a handler on a new instance does not remove an old instance's subscription.
7. Use the existing shared UI scaling helper when available and verified. Capture the app's metrics; avoid another app changing a shared mutable scale during later row creation.
8. Use the verified IL2CPP event adapter. Retain the delegate/subscription identity needed for cleanup.
9. Coordinate text focus through an ownership-aware input gate. Closing one field or app must not release another UI's lock.
10. Reconcile lists by stable ID: add, update, reorder, deactivate/remove missing IDs, and release callbacks on final removal.
11. Route save-game state through the current persistence contract. Global preferences may remain global; history/progress belongs to its exact save identity.

## Failure handling

- Preserve the open-direction visibility sync during close-animation experiments.
- Do not skip framework close bookkeeping without a verified replacement for all affected state.
- Reject stale scene/owner callbacks before accessing Unity objects.
- If loading state fails, show a bounded diagnostic and preserve the write gate; do not save an empty fallback over recoverable data.
- If a resource or helper is missing, report the dependency and keep the affected feature disabled or retain the last usable UI.
- Restore only input, camera, theme, and listener state owned by this app.

## Verify the result

- Open → close phone → reopen repeatedly; navigate away and back to the same app.
- Reload the same save and switch saves; confirm one live app handler and isolated progress.
- Type WASD, numbers, and navigation keys; transfer focus between fields and other open UI.
- Test long labels, empty/large lists, removed rows, scrolling, modal inside/outside clicks, and inline pickers.
- Check supported resolutions/orientations and compare final in-game rendering to the reference.
- Verify fresh-build asset deployment, icon decoding, and cleanup after scene exit.
- Report build results separately from tests that require the running game.

## Read only the needed reference

| Trigger | Reference |
|---|---|
| Blank app, closing, reopening, stale handlers | [Lifecycle and canvas](references/lifecycle-and-canvas.md) |
| Scaling, mockup, typography, visual measurements | [Responsive UI](references/responsive-ui-theme.md) |
| Typing, shortcuts, gamepad/mouse mismatch | [Input focus and controls](references/input-focus-and-controls.md) |
| Buttons, lists, modals, dark theming, hover clipping | [Components and uGUI](references/components-and-ugui.md) |

## Evidence and output

The references retain historical workspace observations as diagnostic leads, with their scope stated.
They are not a runtime certification for S1API 3.2.1. Verify exact signatures before adding calls. <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->
Return the changed responsibilities/files, reason, validation evidence, remaining runtime checks,
and any known compatibility limit. Keep historical incident notes out of the active implementation path.
