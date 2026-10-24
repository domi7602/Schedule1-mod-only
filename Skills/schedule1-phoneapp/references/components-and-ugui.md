# uGUI Components, List Reconciliation, and Modal Hit Testing

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 27 of 28 identifier-shaped tokens resolve (1 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence and use

Use when building phone lists, buttons, inline pickers, and modals. `S1API.UI.UIFactory`, `EventHelper`,
and `ButtonUtils` are inherited integration anchors; inspect the installed DLL and workspace helpers
before using exact parameters or tuple members. No helper table here is a guaranteed 3.2.1 signature. <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->

## Choose and bind components

Historical UIFactory candidates include `Panel`, `Text`, `Button`, `ButtonWithLabel`, and
`RoundedButtonWithLabel`. Resolve the required helper and inspect which returned object owns layout,
which owns interaction, and which owns text. A wrapper root and inner button may be different objects.
Choose rounded or square styling to match the requested design; one workspace's NotesApp style is not
a universal requirement.

Use a verified IL2CPP-safe listener bridge where required. Bind one handler for the lifetime of the
control, retain its removable identity, and detach it on disposal/rebinding. Do not clear unrelated
listeners on shared game controls. For an owned clone, inspect copied listeners and explicitly replace
only the inherited behavior the new control must not execute.

## Scrollable-list structure

Use a `ScrollRect` owner, a stretched viewport with the intended raycast graphic and mask, and a
content rect with a top pivot and width tied to that viewport. A vertical layout can own row heights
and spacing while a content-size fitter owns the vertical content extent. Avoid competing controllers
for the same axis. Connect the viewport/content references explicitly.

For the historical fixed-width tag plus flexible label row, allow the layout to control child widths
without forcing every child to expand equally. Test long text, empty content, one row, enough rows to
scroll, and the content width after the first real layout pass.

## Reconcile the complete data set

A cache that only adds/updates incoming rows leaves removed or filtered items visible.
Use a stable-ID reconciliation contract:

1. Capture a coherent data snapshot and validate uniqueness of IDs.
2. Mark all cached rows unseen for this refresh generation.
3. For each current item, reuse or create its row, update changed display fields, and mark it seen.
4. Deactivate, pool, or dispose rows not seen in the new generation. Clear selection if its item left.
5. Apply the requested ordering using stable IDs, then restore or deliberately adjust scroll position.
6. On reuse, update the row's bound identity and action target. Do not leave a closure pointing to the
   item that occupied the row before pooling.
7. Dispose rows/listeners no longer owned and clear references on container teardown.

Do not modify the dictionary during unsafe enumeration. Pool only when measured churn justifies it,
and bound retained rows for large data sets. Virtualization may be appropriate when the data set is
much larger than the visible viewport.

Updating existing objects reduces churn but does not establish zero allocations: formatting strings,
snapshots, LINQ, closures, and layout rebuilds may still allocate. Profile the real refresh path and
choose a sensible update cadence. Do not destroy/recreate an unchanged list on a timer.

## Modal contract

Use an app-owned full-screen overlay with a backdrop and a foreground card. Arrange them so the card
and its controls intercept the intended pointer events before the backdrop's dismiss action. A
sibling backdrop behind the card is often clearer than relying on bubbling through a parent button.
Inspect the event system's actual dispatch behavior and consume the intended click once.

A `GraphicRaycaster` belongs to a canvas's graphic hit-testing setup. Adding one to an arbitrary card
does not by itself make the card consume a parent backdrop click. Confirm the card has the appropriate
raycast-target graphic/handler and that `CanvasGroup` raycast/interactable settings agree with its
visible state. Avoid unnecessary nested canvases merely to hide an event-routing bug.

On show, acquire modal focus, select the intended first control, and block the underlying app's
interaction through the supported mechanism. On hide, release this modal's ownership and restore a
still-valid prior selection. Closing the phone must dismiss the modal and cancel its pending drag or
text focus. Hidden overlays must not intercept input.

## Inline pickers

Build reusable picker content once, then toggle the selected panel and reconcile its model. Keep one
active picker owner where the design calls for exclusivity. Update layout participation when hidden,
and derive expanded height from content. Avoid generating another hidden picker on every click or
forcing every canvas to rebuild each frame.

## Acceptance checks

Verify add/update/remove/reorder/filter cycles; a removed item must become noninteractive immediately.
Test pooled-row actions, keyboard/controller focus, scroll retention, inside-card clicks, backdrop
clicks, modal replacement, and phone close. Reopen twice and confirm one callback per action.

## Primary sources

- [Unity Graphic Raycaster](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/script-GraphicRaycaster.html).
- [Unity CanvasGroup.blocksRaycasts](https://docs.unity3d.com/ScriptReference/CanvasGroup-blocksRaycasts.html).
