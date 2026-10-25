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

## Native behavior and recovery

- Resolve the actual native behavior controller, including verified child placement; do not assume root-only lookup.
- Adding a network component and assigning an owner does not establish FishNet registration.
- Retain borrowed behavior state until release is confirmed. Failed release blocks replacement navigation.
- Read back critical native mutations. A wrapper call returning normally does not prove the intended field changed.
- Recover only the exact registered NPC under the appropriate authority; refuse unconfirmed recovery instead of silently replacing it.
- Preserve foreign/player ownership and bound retries. Lost native registration can require a scene reload.

## Verify

Select checks for changed behavior: spawn, appearance/mugshot, interaction, scheduled routes,
role behavior, dialogue exit, reload, save switch, restored responses and teardown.
Host/client checks apply only when explicitly in scope.
Include blocked/missing destinations, unavailable assets, and repeated callbacks. Compile against the
actual target assembly before describing an API pattern as supported.

## Read on demand

| Need | Reference |
|---|---|
| API responsibilities, appearance constraints, schedule/lifecycle cautions | [NPC reference](references/s1api-custom-npc-reference.md) |
| Customer, dealer, contact, story, or conditional-presence design | [Role patterns](references/example-project-patterns.md) |

## Evidence and output

The NPC reference records a 2026-10-05 source/DLL mismatch, not current-runtime validation.
Recheck exact members; report role, identity and stage responsibilities under the shared evidence policy.
