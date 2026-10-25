# PhoneApp Lifecycle and Canvas Ownership

> **Canonical home of Rule 10** (OnCreated fires once / never unsubscribe in OnPhoneClosed) — other files reference here instead of duplicating the pattern. verified: 2026-08-20 / 2026-09-11 audit; S1API mechanism re-checked 2026-10-05.
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 35/36 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Evidence and use

Use for registration, blank second opens, orientation, scene reloads, and stale event handlers.
The original workspace observed the one-create/many-open lifecycle in 2026-08/09 and inspected S1API
on 2026-10-05. Public documentation was reviewed on 2026-10-21. Neither establishes the installed
S1API 3.2.1 implementation; confirm callback order and available teardown hooks in the target DLL. <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->

## Discover the actual lifecycle

The inherited integration uses a public non-abstract `S1API.PhoneApp.PhoneApp` subclass discovered
by S1API. Inspect discovery and registration before manually constructing or registering another
instance. Identify whether the runtime instance survives phone closure, scene reload, or UI rebuild.

Historical callback/member lookup candidates include `OnCreated`, `OnCreatedUI`, `OnPhoneOpened`,
`OnPhoneClosed`, `OnDestroyed`, `IsOpen`, `SetAppOpen`, `SpawnUI`, and `SpawnIcon`.
Use their verified visibility/signatures and invocation order; this list is not a replacement API.

Maintain explicit states for instance lifetime, UI-root generation, visibility, focused input, and
session/save identity. A new UI root can require reconstruction without implying a new gameplay save.

## Resource lifetime table

| Resource | Acquire | Release |
|---|---|---|
| Instance update/data event delegate | Once for the owning app instance | Final instance teardown |
| UI hierarchy and component references | Current container generation | Container replacement/final teardown |
| Visible-only expensive work | Open/visible state | Close, disable, focus loss as appropriate |
| Typing/modal input ownership | Focus/modal activation | Its matching blur/close/cancel path |
| Save/session-bound model | Validated session identity | Session change; flush only through permitted save gates |

Do not unsubscribe the only instance update loop on ordinary phone closure if the runtime does not
call `OnCreated` again. That historical bug caused a blank second opening. Conversely, leaving static
subscriptions alive after final teardown leaks old app instances and causes duplicate callbacks.

Store the delegate and publisher associated with the actual owning instance. Subtracting a delegate
bound to a new instance does not remove an old instance's handler. Use a registration flag and exact
matching unsubscribe on teardown; `-=` followed by `+=` alone is not a cross-instance cleanup strategy.

## Isolated UI root

Build one app-owned full-stretch root under the supplied container. Keep it inactive until the
framework reports the app open, then reconcile visibility in both directions. Do not accidentally
hide or destroy the shared `AppsCanvas`, phone screen, or another app's container.

Build the structure once per valid container generation. Opening should refresh/reconcile data and
activate the root; closing should dismiss transient panels, release owned focus, and hide the root.
When the container changes, detach listeners from old controls before discarding their references.

Choose orientation explicitly using the installed API. Historical phone canvases included a 90-degree
rotation, but neither that rotation nor the default portrait/landscape value is universal. Inspect
local rect dimensions and the canvas/scaler before applying another rotation or swapped dimensions.

## Close and reopen correctness

Preserve the framework's app-open transition when implementing custom close behavior. Removing a
`SetAppOpen(false)`-style transition to avoid one transparent frame can leave stale framework state.
Diagnose the visible root, app state, phone animation, backdrop, and update order separately.

An app update that also performs open-state synchronization must keep that lightweight synchronization
reachable while closed. Expensive data scans and rendering can be gated after it. A premature
`if (!open) return` before hiding/synchronizing the root can leave stale content visible.

## Data lifetime and persistence

Classify state before choosing storage. Window preferences may be global; notes, unlocked features,
transactions, world IDs, and progress may belong to a specific save/session. Read the persistence
skill for save identity, unreadable-state handling, write freeze, and slot transitions. UI teardown
must not silently delete durable progress, and a new save must not inherit a prior save's model.

## Reopen and reload verification

Exercise open/close twice, app switch, modal close, phone close while typing, container recreation,
scene reload, and slot A to B to A. Count subscriptions and root instances. Verify old handlers do not
run and current handlers still run after a normal close. Report observed callback order with runtime
versions; a source identifier match is insufficient.

For asset changes, verify fresh-build deployment when authorized, icon decoding and owned-resource
cleanup after scene exit. A source image or successful compile does not prove the installed icon works.

## Primary source and related references

- [S1API PhoneApp public API](https://ifbars.github.io/S1API/api/S1API.PhoneApp.PhoneApp.html).
- [Input ownership](input-focus-and-controls.md).
- [Layout and theming](responsive-ui-theme.md).
