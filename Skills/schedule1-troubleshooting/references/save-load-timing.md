# Save/Load Timing — Readiness, Identity, and Cache Lifetime

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 28/30 identifier-shaped tokens resolve (1 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

> Evidence scope: inherited workspace observations mention an instrumented run on
> 2026-09-29 in which `OnSaveInfoLoaded` was not observed. Neither that trace nor the
> installed game/framework binaries are bundled here. This revision makes no new
> runtime-order claim.

## Establish the current contract

Start with the deployed S1API identity and the matching
[lifecycle catalog](../../schedule1-s1api/references/lifecycle.md).
Verify every event name before subscribing; do not invent `OnPreSave` or `OnSaveLoaded`.
Event existence, an observed callback, completed native loading, and subsystem readiness
are different facts.

The inherited trace reported scene `Main`, then `OnPreLoad`, then `OnLoadComplete`.
It is a test lead, not a universal timeline for each backend, game build, network role,
or same-slot reload. An empty player-owned list can also be valid game state.

## Readiness workflow

1. Identify the world and reset/invalidate the previous world's transient references at
   the verified transition boundary.
2. Enter a loading state that prevents writes based on uninitialized or failed input.
3. Load/validate this world's persisted data with explicit result states.
4. Observe the relevant completion event and separately check the dependencies this
   feature needs: local player, ownership, registry, UI root, or replicated state.
5. Publish a ready snapshot only for the same world/session generation.
6. Refresh consumers idempotently and retain diagnostics for rejected or delayed input.

Use `OnPreLoad` and `OnLoadComplete` only after their current contracts have been confirmed.
Do not keep an obsolete `OnSaveInfoLoaded` example as the active recipe beside a warning
that it did not fire. A subscribed event can exist without satisfying the feature's needs.

## Why proxy text cannot prove event behavior

IL2CPP interop methods often invoke native functions. A full-text search that finds no
managed call to a native event does not prove the event is never raised, nor does it rule
out a framework wiring issue. Use matched implementation evidence or a bounded live trace.
Record subscription time, scene, world, network role, callback counts, and observation end.
Zero callbacks means none were observed during that trace, not that none can occur.

## Polling and event trade-offs

Prefer a reliable, verified lifecycle event when it expresses the actual readiness needed.
Where it does not, use a bounded readiness check with a deadline and cancellation on a
new world generation. Do not use arbitrary long sleeps or poll forever.
Never cache an early empty list permanently just because it was the first access.
An event can trigger a re-check; it need not be treated as proof that every subsystem is ready.

## Persistence and restoration ownership

Separate persistent domain data from scene references and presentation caches.
Scene unload may invalidate scene references, including objects behind cached singletons.
It must not blindly clear the data that an imminent save still needs.
Resolve persistent object identity at the lifecycle stage where it is actually assigned;
`Awake` is not automatically sufficient for a saved GUID.
Native buildable restoration and mod-side spawning must not both create the same object.
See [buildable restoration](../../schedule1-persistence/references/buildable-restore.md).

## Save-side hooks

Inspect the actual start/completion callbacks and when the native save captures data.
A completion notification does not prove that every sidecar write succeeded.
Keep dirty data and explicit failure state after a failed last write; never report a clean
shutdown save unless the intended target was confirmed.
For framework `Saveable` objects, verify the current load-order mechanism and callbacks
from the target version instead of guessing an enum or override.

## Optional framework integration

If S1API is optional, isolate framework-dependent loading and calls so missing assemblies
are handled before code requiring their types is resolved.
Discover the actual event surface and delegate signature. Use a tested readiness alternative
when the required event is absent; do not silently subscribe to a similarly named event.

## Multiplayer

`OnLoadComplete` is not a universal replication barrier. Observe the relevant host-owned
state, reconcile client caches after subsequent replication, and reject stale session data.
A client-side write may be ignored, remain local, or be overwritten depending on the
actual field contract. See [FishNet diagnosis](fishnet-syncvar-diagnosis.md).

## Verification matrix

Verify a new world, existing world, empty-but-valid world, same-world reload, another world,
replacement in the same slot, return to menu, failure during load, and cancellation of a
pending refresh. Include host/client and late join when the feature supports multiplayer.
Log observed order and readiness independently; record all untested variants explicitly.
