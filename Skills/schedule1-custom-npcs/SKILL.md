---
name: schedule1-custom-npcs
description: "Build and restore custom Schedule I NPCs."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# Schedule I — Custom NPCs

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Use the installed S1API assembly as the signature authority; the requested target is S1API 3.2.1. <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->

## Establish the NPC contract

1. Identify physical versus non-physical presence and customer/dealer/supplier/story roles.
2. Define stable identity, save ownership, spawn prerequisites, and desired runtime interactions.
3. Inspect the current NPC base, prefab builder, schedule wrappers, and lifecycle callbacks.
4. Compare checked-in source with the deployed assembly before using visibility or signature claims.
5. Confirm required assets, destination reachability, and multiplayer authority.

## Separate responsibilities

| Stage | Responsibility |
|---|---|
| Prefab configuration | Stable identity, role infrastructure, spawn, appearance defaults, relationship/customer/dealer defaults, schedule definitions, required action components |
| Instance creation | Base initialization, appearance application, supported schedule activation, dialogue and event wiring |
| Load restoration | Runtime references, state affected by loading, saved message-response callbacks |
| Final teardown | Detach owned delegates, release leases and cached ownership, remove owned runtime resources |

Let S1API own custom NPC creation. Do not create a second instance manually or change its persistent ID
as an incidental refactor. Role properties must not depend on initialization that occurs too late.

## Implement in order

1. Configure persistent defaults through the verified prefab path.
2. Prefer supported schedule wrappers over low-level action specs for ordinary movement and interaction.
3. Add only verified action prerequisites. Check customer/dealer deal signals against the selected role.
4. Apply runtime appearance through the supported build/apply path.
5. Enable the schedule through its public contract. **Do not call `Schedule.InitializeActions()` from mod code based on old examples:** inherited source inspection found it internal and framework-owned.
6. Bind dialogue callbacks and release dialogue from a safe choice/completion path; avoid reentrant override changes during node display.
7. Rebind persisted message-response behavior after loading and make one-time introductions idempotent.
8. Add ownership-aware cleanup before feature expansion; stale teardown must not erase a replacement instance's registration.

## Relocating an existing service NPC

1. Trace every consumer of the current position before changing it: prefab spawn, waiting schedule, return/dismiss, saved-position restoration, and any vehicle route or parking target. Separate a pedestrian waiting point from a shared vehicle entry coordinate so moving the person does not break a drivable route.
2. Derive a lateral offset from observed or documented entry orientation and nearby exits, not an arbitrary world axis. Keep the candidate outside the driveway; confirm ground clearance visually before claiming the new placement is accepted.
3. Wire spawn, waiting schedule and return to one NPC-specific position. Add regressions proving all three use it and vehicle targets remain unchanged; read back a warp with a small explicit tolerance rather than logging success just because the setter returned.
4. Inspect load ordering before promising immediate relocation in an existing save: persisted position can override prefab defaults. Exercise the existing-save path or explicitly state the schedule/return event that applies the change. Do not add an unconditional load-time teleport that could interrupt an occupied vehicle or active service.
5. Treat the owner's screenshot and explicit approval as acceptance of placement, not proof of boarding, pathfinding or reload. Close the requested relocation without reopening unrelated taxi behavior.

## Native behavior and recovery

- Resolve the actual native behavior controller, including verified child placement; do not assume root-only lookup.
- Adding a network component and assigning an owner does not establish FishNet registration.
- Trace a missing-native-behavior warning into its fallback before calling it the cause of a service failure. An intentional direct-navigation fallback can successfully board and transport the player; correlate the failing operation and its terminal result instead of adding a patrol/network component merely to remove the warning.
- Retain borrowed behavior state until release is confirmed. Failed release blocks replacement navigation.
- Read back critical native mutations. A wrapper call returning normally does not prove the intended field changed.
- Recover only the exact registered NPC under the appropriate authority; refuse unconfirmed recovery instead of silently replacing it.
- Preserve foreign/player ownership and bound retries. Lost native registration can require a scene reload.
- A logged recovery episode is not a manoeuvre. A stall-history gate can classify a new stall as a recurrence on nearly the same ground, skip reverse and rescue, and run into a terminal give-up while still emitting an episode line each time. Read position, speed and wheel output next to every episode line before calling a recovery ineffective.
- Soft lane correction can be deliberately off while a borrowed patrol driver owns the wheel, leaving only the hard snap. That snap needs a free spot on the route, so "no free spot" degrades to handing the car back to the game's own recovery. A missing soft-correction line is not evidence that the keeper failed.
- A refused free-spot rescue is not proof that the car is not wedged. A car at full throttle with zero speed beside the route still needs one bounded physical reverse before the terminal give-up, and that reverse must own the re-dispatch; gate it to one attempt per operation so the stall branch cannot turn into a reverse ladder.
- Compare the car's position with the route's first point before granting a manoeuvre. A reverse can move the car farther from the route start while still looking like progress, so judge the manoeuvre by the resulting distance to the route, not by the fact that it ran.
- Separate a call-out from a ride when reading a service-drive log: a destination accepted with a "takes effect on the next ride, no ride is running" note means nobody is aboard, so the elapsed interval is not travel time. Boarding has its own line.

## Verify

Select checks for changed behavior: spawn, appearance/mugshot, interaction, scheduled routes,
role behavior, dialogue exit, reload, save switch, restored responses and teardown.
Host/client checks apply only when explicitly in scope.
Include blocked/missing destinations, unavailable assets, and repeated callbacks. Compile against the
actual target assembly before describing an API pattern as supported.
For a driven service NPC, drive a second consecutive destination from the previous drop-off —
a first trip from a fresh spawn never exercises turning around at the arrival point.

## Read on demand

| Need | Reference |
|---|---|
| API responsibilities, appearance constraints, schedule/lifecycle cautions | [NPC reference](references/s1api-custom-npc-reference.md) |
| Customer, dealer, contact, story, or conditional-presence design | [Role patterns](references/example-project-patterns.md) |

## Evidence and output

The NPC reference records a 2026-10-05 source/DLL mismatch, not current-runtime validation.
Recheck exact members; report role, identity and stage responsibilities under the shared evidence policy.
