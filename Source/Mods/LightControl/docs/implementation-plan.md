# Gameplay integration plan

## Current boundary

The existing Core and diagnostic assembly compile, but no gameplay adapter is implemented or
deployed. The owner authorized taking over implementation; the preceding layout-only decision
explicitly excluded deployment. A diagnostic or gameplay deployment still needs its scope agreed.

## Discovery evidence

Read-only research identified candidate integration points. Parent spot-checks confirm these
members in the local generated interop sources:

- `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne.EntityFramework/BuildableItem.cs`: `IsDestroyed` (897), `GUID` (946), `IsCulled` (970), `InitializeBuildableItem` (1393).
- `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne.Misc/ToggleableLight.cs`: `lightSources` (64), `lightSurfacesMeshes` (79), `MaterialIndex` (94), `lightOnMat` (107), `lightOffMat` (122).
- `ThirdParty/S1API/S1API/Lifecycle/GameLifecycle.cs`: `OnPreLoad` (50), `OnLoadComplete` (69), `OnPreSceneChange` (88), `OnSaveStart` (126).

These generated proxies establish signatures, not native execution order, synchronization or
actual scene/material contents. A GUID-shaped member is a candidate identity; confirm assignment,
serialization and reload behavior before advertising durable lamp identity. Do not infer native
RPC behavior from method names or wrapper caller counts.

## First evidence gate

Inspect a placed grow light and a switchable fixture: actual component relationships, light
writers, material slots, shader properties, culling and switching. Existing diagnostic discovery
must itself be checked for usable IL2CPP enumeration and object identity coverage before asking
for a game restart. Generic Unity Light listing alone does not establish logical fixtures.
No automatic broad per-frame scan. Do not alter growth, exposure, electricity or native on/off
state merely to demonstrate visual color control.

## Proposed first usable slice

One verified placed-lamp family connected end-to-end: registration, original/color/intensity
controls, native-state-aware visual adapter, minimal phone UI, save-slot isolation and reversible
cleanup. Use the existing pure Core rather than duplicating its state model. Extend executable
checks for baseline ownership, restoration, identity ambiguity and session transitions before
production wiring. Inspect native interaction ownership before selecting or suppressing input;
do not redefine the original brief's normal interaction/sprint behavior by guesswork.

This slice is not the full requested release. Still pending: all other lamp families and emission
coverage, fixed-world identity, rooms/switch groups, native remote switching, multiplayer
handshake/transport/permissions, effect integration, localization acceptance, performance and
full gameplay/package acceptance. Keep those requirements from the original brief rather than
silently removing them. Technical transport selection needs a feasibility experiment; do not
ask the owner to select a library before comparing supported options.

## Original scope

See `upstream/IMPLEMENTATION_BRIEF.txt` and `upstream/REQUIREMENTS.md`. Do not promote the imported
model's tests into proof of real lighting, persistence or multiplayer behavior.
